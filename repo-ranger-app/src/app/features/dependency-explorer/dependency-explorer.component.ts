import { PanelModule } from 'primeng/panel';
import { DependencyVm } from './../../generated/model/dependency-vm';
import { Component, OnInit } from '@angular/core';
import { ProjectVm, ProjectsService, ProjectsVm, RepositoriesService, RepositoriesVm, RepositoryVm, SourceVm } from '../../generated';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { DependencyTableComponent } from './dependency-table/dependency-table.component';
import { SplitterModule } from 'primeng/splitter';
import { CardModule } from 'primeng/card';
import { MultiSelectChangeEvent, MultiSelectModule } from 'primeng/multiselect';
import { InputTextModule } from 'primeng/inputtext';
import { AccordionModule } from 'primeng/accordion';
import { DependencyDetailsViewComponent } from './dependency-details-view/dependency-details-view.component';

@Component({
  selector: 'app-dependency-explorer',
  standalone: true,
  templateUrl: './dependency-explorer.component.html',
  styleUrl: './dependency-explorer.component.scss',
  imports: [
    DependencyTableComponent,
    PanelModule,
    CardModule,
    MultiSelectModule,
    InputTextModule,
    FormsModule,
    AccordionModule,
    DependencyDetailsViewComponent
  ],
})
export class DependencyExplorerComponent implements OnInit {

  selectedRepositories: RepositoryVm[] = [];
  repositories: RepositoryVm[] = [];

  selectedProjects: ProjectVm[] = [];
  projects: ProjectVm[] = [];
  projectsVm!: ProjectsVm;

  filterIcon: string = 'pi-filter';

  constructor(private readonly repositoryService: RepositoriesService, private readonly projectsService: ProjectsService) {}

  ngOnInit(): void {
    this.repositoryService.apiRepositoriesGet().subscribe(({
      next: (repositoriesVm) => this.handleRepositoriesSuccess(repositoriesVm),
      error: (error) => this.handleError(error)
    }));

    this.projectsService.apiProjectsGet().subscribe(({
      next: (projectsVm) => this.handleProjectsSuccess(projectsVm),
      error: (error) => this.handleError(error)
    }));
  }

  handleRepositoriesSuccess(repositoriesVm: RepositoriesVm): void {
    if(!repositoriesVm.repositories) return;
    this.repositories = repositoriesVm.repositories;
  }

  handleProjectsSuccess(projectsVm: ProjectsVm): void {
    if(!projectsVm.projects) return;
    this.projects = projectsVm.projects;
    this.projectsVm = projectsVm;
  }

  handleError(error: any): void {
    console.error('Error fetching data', error)
  }

  selectedRepositoriesChanged($event: MultiSelectChangeEvent) {
    this.ToggleFilterIcon();
    this.selectedRepositories = $event.value;

    if(!this.selectedRepositories) {
      if(!this.projectsVm.projects) return;

      this.projects = this.projectsVm.projects;
      return;
    }
    else {
      const repositoryIds: string[] = this.selectedRepositories
      .filter(r => r.id !== undefined)
      .map(r => r.id!);

      this.projects = this.projectsVm.projects ?? [];
      this.projects = this.projects.filter(p => repositoryIds.includes(p.repositoryId!))
    }

  }

  selectedProjectsChanged($event: MultiSelectChangeEvent) {
    this.ToggleFilterIcon();
    this.selectedProjects = $event.value;
  }

  private ToggleFilterIcon() {
    if (this.hasSelectedRepositories() || this.hasSelectedProjects()) {
      this.filterIcon = 'pi-filter-fill';
    }
    else {
      this.filterIcon = 'pi-filter';
    }
  }

  private hasSelectedRepositories(): boolean {
    return this.selectedRepositories.length > 0;
  }

  private hasSelectedProjects(): boolean {
    return this.selectedProjects.length > 0;
  }
}
