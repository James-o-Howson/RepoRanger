import { PanelModule } from 'primeng/panel';
import { Component, OnInit } from '@angular/core';
import {
  FormsModule,
} from '@angular/forms';
import { DependencyTableComponent } from './dependency-table/dependency-table.component';
import { CardModule } from 'primeng/card';
import { ChipModule } from 'primeng/chip';
import { MultiSelectChangeEvent, MultiSelectModule } from 'primeng/multiselect';
import { InputTextModule } from 'primeng/inputtext';
import { AccordionModule } from 'primeng/accordion';
import { DependencyDetailsViewComponent } from './dependency-details-view/dependency-details-view.component';
import { CommonModule } from '@angular/common';
import { ListProjectsQuery, ListRepositoriesQuery, ProjectVm, ProjectsClient, ProjectsVm, RepositoriesClient, RepositorySummariesVm, RepositorySummaryVm } from '../../api-client';
import { FilterService } from './filter.service';

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
    DependencyDetailsViewComponent,
    ChipModule,
    CommonModule
  ],
})
export class DependencyExplorerComponent implements OnInit {

  selectedRepositories: RepositorySummaryVm[] = [];
  repositories: RepositorySummaryVm[] = [];

  selectedProjects: ProjectVm[] = [];
  projects: ProjectVm[] = [];
  projectsVm!: ProjectsVm;

  filterIcon: string = 'pi-filter';

  constructor(
    private readonly projectsClient: ProjectsClient, 
    private readonly repositoriesClient: RepositoriesClient,
    private readonly filterService: FilterService
  ) {}

  ngOnInit(): void {

    this.repositoriesClient.repositories_List(new ListRepositoriesQuery()).subscribe(({
      next: (repositoriesVm) => this.handleRepositoriesSuccess(repositoriesVm),
        error: (error) => this.handleError(error)
    }));

    this.projectsClient.projects_List(new ListProjectsQuery()).subscribe(({
      next: (projectsVm) => this.handleProjectsSuccess(projectsVm),
      error: (error) => this.handleError(error)
    }));
  }

  handleRepositoriesSuccess(repositoriesVm: RepositorySummariesVm): void {
    if(!repositoriesVm.repositorySummaries) return;
    this.repositories = repositoriesVm.repositorySummaries;
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

    if(!this.selectedRepositories || this.selectedRepositories.length === 0) {
      if(!this.projectsVm.projects) return;

      this.projects = this.projectsVm.projects;
      this.filterService.setSelectedRepositories([]);
      return;
    }
    else {
      const repositoryIds: number[] = this.selectedRepositories
        .filter(r => r.id !== undefined)
        .map(r => r.id!);

      this.projects = this.projectsVm.projects ?? [];
      this.projects = this.projects.filter(p => repositoryIds.includes(p.repositoryId!))
      this.filterService.setSelectedRepositories(repositoryIds);
    }
  }

  selectedProjectsChanged($event: MultiSelectChangeEvent) {
    this.ToggleFilterIcon();
    this.selectedProjects = $event.value;

    const projectIds: number[] = this.selectedProjects
        .filter(r => r.id !== undefined)
        .map(r => r.id!);

    this.filterService.setSelectedProjects(projectIds);
  }

  hasSelectedRepositories(): boolean { 
    return this.selectedRepositories.length > 0;
  }

  hasSelectedProjects(): boolean { 
    return this.selectedProjects.length > 0;
  }

  repositoriesChipOverflowCount(): number {
    let count: number = this.selectedRepositories.length;
    return count > 3 ? count - 3 : 0;
  }

  projectsChipOverflowCount(): number {
    let count: number = this.selectedProjects.length;
    return count > 3 ? count - 3 : 0;
  }

  chipOverflowCountText(count: number): string {
    return count == 1 ? '1 other...' : `${count} others...`;
  }

  private ToggleFilterIcon() {
    if (this.hasSelectedRepositories() || this.hasSelectedProjects()) {
      this.filterIcon = 'pi-filter-fill';
    }
    else {
      this.filterIcon = 'pi-filter';
    }
  }
}