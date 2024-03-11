
import { SelectedDependencyService } from './selected-dependency.service';
import { CardModule } from 'primeng/card';
import { Component, OnInit } from '@angular/core';
import { Table, TableLazyLoadEvent, TableModule } from 'primeng/table';
import { DependencyInstanceVm, DependencyInstancesClient, FilterOperator, MatchMode, PaginatedFilter, PaginatedListOfDependencyInstanceVm, SearchDependencyInstancesWithPaginationQuery, SortOrder } from '../../../api-client'
import { ButtonModule } from 'primeng/button';
import { PaginatorModule } from 'primeng/paginator';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule } from '@angular/forms';
import { FilterMetadata } from 'primeng/api/filtermetadata';

@Component({
  selector: 'app-dependency-table',
  standalone: true,
  imports: [
    TableModule,
    ButtonModule,
    PaginatorModule,
    InputTextModule,
    FormsModule,
    CardModule,
  ],
  templateUrl: './dependency-table.component.html',
  styleUrl: './dependency-table.component.scss',
})
export class DependencyTableComponent implements OnInit {
  pageSize: number = 25;
  totalRecords: number = 0;
  loading: boolean = false;
  paginatedDependencies!: PaginatedListOfDependencyInstanceVm;
  items!: DependencyInstanceVm[];
  selectedDependency: DependencyInstanceVm | null = null;

  constructor(
    private readonly apiClient: DependencyInstancesClient,
    private readonly selectedDependencyService: SelectedDependencyService
  ) {}

  ngOnInit(): void {
    this.loading = true;
  }

  searchDependenciesSuccess(paginatedDependencies: PaginatedListOfDependencyInstanceVm): void {
    if (!paginatedDependencies.items) return;
    this.items = paginatedDependencies.items;
    this.totalRecords = paginatedDependencies.totalCount ?? 0;

    this.loading = false;
    console.log(paginatedDependencies);
  }

  handleError(error: any): void {
    this.loading = false;
    console.error('Error while trying to load dependencies', error);
  }

  loadDependencies($event: TableLazyLoadEvent) {
    this.loading = true;
    const filters = this.getFilters($event.filters);

    this.apiClient.dependencyInstances_Search({
      pageNumber: this.calculatePageNumber($event.first),
          pageSize: this.pageSize,
          sortField: this.getFieldName($event.sortField),
          sortOrder: this.getSortOrder($event.sortOrder),
          filters: filters,
    } as SearchDependencyInstancesWithPaginationQuery)
    .subscribe({
          next: (paginatedDependencies) =>
            this.searchDependenciesSuccess(paginatedDependencies),
          error: (error) => this.handleError(error),
        });
  }

  clear(table: Table) {
    table.clear();
  }

  selectedDependencyChanged(selectedDependency: DependencyInstanceVm | null) {
    this.selectedDependencyService.setSelectedDependency(selectedDependency);
  }

  private calculatePageNumber(first: number | undefined): number {
    if (first === undefined) return 1;
    return Math.floor(first / this.pageSize) + 1;
  }

  private getFieldName(
    sortField: string | string[] | null | undefined
  ): string {
    switch (sortField) {
      case 'name':
        return 'Name';
      case 'version':
        return 'Version';
      default:
        return 'Name';
    }
  }

  private getSortOrder(sortOrder: number | undefined | null): SortOrder {
    switch (sortOrder) {
      case 1:
        return SortOrder.Ascending;
      case -1:
        return SortOrder.Descending;
      default:
        return SortOrder.Ascending;
    }
  }

  private getFilters(
    filters:
      | { [s: string]: FilterMetadata | FilterMetadata[] | undefined }
      | undefined
  ): { [key: string]: Array<PaginatedFilter> } {
    const result: { [key: string]: PaginatedFilter[] } = {};

    if (!filters) return { '': [] };

    for (const [key, value] of Object.entries(filters)) {
      if (!value) {
        result[this.getFieldName(key)] = [];
        continue;
      }

      let paginatedFilters: Array<PaginatedFilter>;
      if (Array.isArray(value)) {
        paginatedFilters = value
          .filter((filter) => filter.value !== null)
          .map((filter) => (new PaginatedFilter({
            matchMode: this.getMatchMode(filter.matchMode),
            operator: this.getOperator(filter.operator),
            value: filter.value,
          })));
      } else {
        if (value === null) continue;
        paginatedFilters = [new PaginatedFilter({
          matchMode: this.getMatchMode(value.matchMode),
          operator: this.getOperator(value.operator),
          value: value.value,
        })];
      }

      if (paginatedFilters.length === 0) continue;

      result[this.getFieldName(key)] = paginatedFilters;
    }

    return result;
  }

  private getMatchMode(matchMode: string | null | undefined): MatchMode | undefined {
    switch (matchMode) {
      case 'startsWith':
        return MatchMode.StartsWith;
      case 'contains':
        return MatchMode.Contains;
      case 'notContains':
        return MatchMode.NotContains;
      case 'endsWith':
        return MatchMode.EndsWith;
      case 'equals':
        return MatchMode.Equals;
      case 'notEquals':
        return MatchMode.NotEquals;
      default:
        return undefined;
    }
  }

  private getOperator(operator: string | null | undefined): FilterOperator | undefined {
    switch (operator) {
      case 'and':
        return FilterOperator.And;
      case 'or':
        return FilterOperator.Or;
      default:
        return FilterOperator.And;
    }
  }
}
