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
import { Client, ListProjectsQuery, ListRepositoriesQuery, ProjectVm, ProjectsVm, RepositoriesVm, RepositoryVm } from '../../api-client';

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

  selectedRepositories: RepositoryVm[] = [];
  repositories: RepositoryVm[] = [];

  selectedProjects: ProjectVm[] = [];
  projects: ProjectVm[] = [];
  projectsVm!: ProjectsVm;

  filterIcon: string = 'pi-filter';

  constructor(private readonly apiClient: Client) {}

  ngOnInit(): void {

    this.apiClient.repositories(new ListRepositoriesQuery()).subscribe(({
      next: (repositoriesVm) => this.handleRepositoriesSuccess(repositoriesVm),
        error: (error) => this.handleError(error)
    }));

    this.apiClient.projects(new ListProjectsQuery()).subscribe(({
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
      const repositoryIds: number[] = this.selectedRepositories
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