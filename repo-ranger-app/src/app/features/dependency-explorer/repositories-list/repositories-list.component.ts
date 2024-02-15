import {
  AfterViewInit,
  Component,
  EventEmitter,
  Input,
  Output,
  ViewChild,
} from '@angular/core';
import {
  MatTable,
  MatTableDataSource,
  MatTableModule,
} from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { formatDate } from '@angular/common';
import { ErrorHandlerService } from '../../../core/error-handler.service';
import { RepositoryVm } from '../../../generated/model/repository-vm';
import { RepositoriesService } from '../../../generated/api/repositories.service';
import { RepositoriesVm } from '../../../generated/model/repositories-vm';
import { GetRepositoriesBySourceIdQuery } from '../../../generated/model/models';

@Component({
  selector: 'app-repositories-list',
  standalone: true,
  imports: [MatTableModule, MatPaginatorModule, MatSortModule],
  templateUrl: './repositories-list.component.html',
  styleUrl: './repositories-list.component.scss',
})
export class RepositoriesListComponent implements AfterViewInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatTable) table!: MatTable<RepositoryVm>;

  @Input() set sourceId(value: string | undefined) {
    this.sourceIdValue = value;

    if (!this.sourceIdValue) return;
    this.LoadRepositoriesForSource(this.sourceIdValue);
  }

  @Output() repositorySelected = new EventEmitter<RepositoryVm>();

  private sourceIdValue: string | undefined = '';

  constructor(
    private readonly repositoryService: RepositoriesService,
    private readonly errorHandlerService: ErrorHandlerService
  ) {
    this.dataSource = new MatTableDataSource<RepositoryVm>([]);
  }

  dataSource: MatTableDataSource<RepositoryVm>;
  selectedRepository?: RepositoryVm | null;
  displayedColumns = [
    'id',
    'parseTime',
    'name',
    'defaultBranchName',
    'remoteUrl',
  ];

  private LoadRepositoriesForSource(id: string) {
    this.repositoryService
      .apiRepositoriesPost(this.createGetRepositoriesBySourceIdQuery(id))
      .subscribe({
        next: (repositoriesVm) =>
          this.handlepApiRepositoriesPostSuccess(repositoriesVm),
        error: (error) => this.errorHandlerService.handleError(error),
      });
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  handlepApiRepositoriesPostSuccess(repositoriesVm: RepositoriesVm): void {
    console.log(repositoriesVm);

    if (!repositoriesVm.repositories) return;

    this.dataSource = new MatTableDataSource<RepositoryVm>(
      repositoriesVm.repositories
    );
    this.dataSource.sort = this.sort;
  }

  createGetRepositoriesBySourceIdQuery(
    sourceId: string
  ): GetRepositoriesBySourceIdQuery {
    const query: GetRepositoriesBySourceIdQuery = {
      sourceId: sourceId,
    };

    return query;
  }

  selectRepository(row: RepositoryVm) {
    this.selectedRepository = row;
    this.repositorySelected.emit(row);
  }

  isSelected(repository: RepositoryVm) {
    return this.selectedRepository === repository;
  }

  getDataSourceLength(): number {
    return this.dataSource?.data?.length ?? 0;
  }

  toLocalDateTime(value?: string | Date | null): string | null {
    if (value) {
      if (
        value.toString().indexOf('Z') > 0 ||
        value.toString().indexOf('+') > 0
      ) {
        return formatDate(value, 'dd/MM/yyyy HH:mm:ss', 'en-AU', '+0800');
      } else {
        return formatDate(
          value.toString() + 'Z',
          'dd/MM/yyyy HH:mm:ss',
          'en-AU',
          '+0800'
        );
      }
    }
    return null;
  }
}
