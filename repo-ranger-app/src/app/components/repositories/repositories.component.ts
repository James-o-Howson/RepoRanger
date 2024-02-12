import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { GetRepositoriesBySourceIdQuery, RepositoriesService, RepositoriesVm, RepositoryVm } from '../../generated';
import { ErrorHandlerService } from '../../core/error-handler.service';
import { MatTable, MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { formatDate } from '@angular/common';

@Component({
  selector: 'app-repositories',
  standalone: true,
  imports: [MatTableModule, MatTable, MatPaginator, MatSort],
  templateUrl: './repositories.component.html',
  styleUrl: './repositories.component.scss'
})
export class RepositoriesComponent implements OnInit, AfterViewInit {

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatTable) table!: MatTable<RepositoryVm>;

  dataSource: MatTableDataSource<RepositoryVm>;
  selectedRepository?: RepositoryVm | null;

  displayedColumns = ['id', 'parseTime', 'name', 'defaultBranchName', 'remoteUrl'];

  constructor(
    private readonly repositoryService: RepositoriesService,
    private readonly errorHandlerService: ErrorHandlerService) {
    this.dataSource = new MatTableDataSource<RepositoryVm>([]);
  }

  ngOnInit(): void {
    this.repositoryService.apiRepositoriesPost(this.createGetRepositoriesBySourceIdQuery('6282E78F-0F7E-40A6-9ED9-552EF4A9BCBC')).subscribe({
      next: (repositoriesVm) => this.handlepAiRepositoriesPostSuccess(repositoriesVm),
      error: (error) => {
        this.errorHandlerService.handleError(error)
      }
    });
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  handlepAiRepositoriesPostSuccess(repositoriesVm: RepositoriesVm): void {
    console.log(repositoriesVm)

    if (!repositoriesVm.repositories) return;

    this.dataSource = new MatTableDataSource<RepositoryVm>(repositoriesVm.repositories);
    this.dataSource.sort = this.sort;
  }

  createGetRepositoriesBySourceIdQuery(sourceId: string): GetRepositoriesBySourceIdQuery {
    const query: GetRepositoriesBySourceIdQuery = {
      sourceId: sourceId
    };

    return query;
  }

  selectRepository(row: RepositoryVm) {
    
  }

  isSelected(repository: RepositoryVm) {
    return (this.selectedRepository === repository);
  }

  getDataSourceLength(): number {
    return this.dataSource?.data?.length ?? 0;
  }

  toLocalDateTime(value?: string | Date | null): string | null {
    if (value) {
      if (value.toString().indexOf('Z') > 0 || value.toString().indexOf('+') > 0) {
        return formatDate(value, 'dd/MM/yyyy HH:mm:ss', 'en-AU', '+0800');
      } else {
        return formatDate(value.toString() + 'Z', 'dd/MM/yyyy HH:mm:ss', 'en-AU', '+0800');
      }
    }
    return null;
  }
}
