import { Component, OnInit } from '@angular/core';
import { TableLazyLoadEvent, TableModule } from 'primeng/table';
import { DependenciesService, DependencyVm, DependencyVmPaginatedList, SearchDependenciesWithPaginationQuery } from '../../../generated';
import { ButtonModule } from 'primeng/button';
import { PaginatorModule } from 'primeng/paginator';

@Component({
  selector: 'app-dependency-table',
  standalone: true,
  imports: [TableModule, ButtonModule, PaginatorModule],
  templateUrl: './dependency-table.component.html',
  styleUrl: './dependency-table.component.scss'
})
export class DependencyTableComponent implements OnInit {

  pageSize: number = 25;
  totalRecords: number = 0;
  loading: boolean = false;

  paginatedDependencies!: DependencyVmPaginatedList;
  items!: DependencyVm[];

  constructor(private readonly dependenciesService: DependenciesService) { }

  ngOnInit(): void {
    this.loading = true;
  }

  searchDependenciesSuccess(paginatedDependencies: DependencyVmPaginatedList): void {
    if(!paginatedDependencies.items) return;
    this.items = paginatedDependencies.items;
    this.totalRecords = paginatedDependencies.totalCount ?? 0;

    this.loading = false;
    console.log(paginatedDependencies);
  }

  handleError(error: any): void {
    this.loading = false;
    console.error("Error while trying to load dependencies", error)
  }

  loadDependencies($event: TableLazyLoadEvent) {
    this.loading = true;
    this.dependenciesService.apiDependenciesSearchPost({
      pageNumber: this.calculatePageNumber($event.first),
      pageSize: this.pageSize
    } as SearchDependenciesWithPaginationQuery).subscribe({
      next: (paginatedDependencies) => this.searchDependenciesSuccess(paginatedDependencies),
      error: (error) => this.handleError(error)
    });
  }

  private calculatePageNumber(first: number | undefined): number {
    if(first === undefined ) return 1;
    return Math.floor(first / this.pageSize) + 1
  }
}
