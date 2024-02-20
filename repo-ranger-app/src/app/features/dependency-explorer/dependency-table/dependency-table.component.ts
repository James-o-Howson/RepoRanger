import { SelectedDependencyService } from './selected-dependency.service';
import { CardModule } from 'primeng/card';
import { Component, OnInit } from '@angular/core';
import { Table, TableLazyLoadEvent, TableModule } from 'primeng/table';
import {
  DependenciesService,
  DependencyVm,
  DependencyVmPaginatedList,
  SearchDependenciesWithPaginationQuery,
  SortOrder,
  PaginatedFilter,
  MatchMode,
  FilterOperator,
} from '../../../generated';
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
  paginatedDependencies!: DependencyVmPaginatedList;
  items!: DependencyVm[];
  selectedDependency: DependencyVm | null = null;

  constructor(
    private readonly dependenciesService: DependenciesService,
    private readonly selectedDependencyService: SelectedDependencyService
  ) {}

  ngOnInit(): void {
    this.loading = true;
  }

  searchDependenciesSuccess(
    paginatedDependencies: DependencyVmPaginatedList
  ): void {
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

    this.dependenciesService
      .apiDependenciesSearchPost({
        pageNumber: this.calculatePageNumber($event.first),
        pageSize: this.pageSize,
        sortField: this.getFieldName($event.sortField),
        sortOrder: this.getSortOrder($event.sortOrder),
        filters: filters,
      } as SearchDependenciesWithPaginationQuery)
      .subscribe({
        next: (paginatedDependencies) =>
          this.searchDependenciesSuccess(paginatedDependencies),
        error: (error) => this.handleError(error),
      });
  }

  clear(table: Table) {
    table.clear();
  }

  selectedDependencyChanged(selectedDependency: DependencyVm | null) {
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
        return SortOrder.NUMBER_0;
      case -1:
        return SortOrder.NUMBER_1;
      default:
        return SortOrder.NUMBER_0;
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
          .map((filter) => ({
            matchMode: this.getMatchMode(filter.matchMode),
            operator: this.getOperator(filter.operator),
            value: filter.value,
          }));
      } else {
        if (value === null) continue;
        paginatedFilters = [
          {
            matchMode: this.getMatchMode(value.matchMode),
            operator: this.getOperator(value.operator),
            value: value.value,
          },
        ];
      }

      if (paginatedFilters.length === 0) continue;

      result[this.getFieldName(key)] = paginatedFilters;
    }

    return result;
  }

  private getMatchMode(matchMode: string | undefined): MatchMode | undefined {
    switch (matchMode) {
      case 'startsWith':
        return MatchMode.NUMBER_0;
      case 'contains':
        return MatchMode.NUMBER_1;
      case 'notContains':
        return MatchMode.NUMBER_2;
      case 'endsWith':
        return MatchMode.NUMBER_3;
      case 'equals':
        return MatchMode.NUMBER_4;
      case 'notEquals':
        return MatchMode.NUMBER_5;
      default:
        return undefined;
    }
  }

  private getOperator(
    operator: string | undefined
  ): FilterOperator | undefined {
    switch (operator) {
      case 'and':
        return FilterOperator.NUMBER_0;
      case 'or':
        return FilterOperator.NUMBER_1;
      default:
        return undefined;
    }
  }
}
