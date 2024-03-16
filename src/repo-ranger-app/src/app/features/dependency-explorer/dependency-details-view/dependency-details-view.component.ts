import { ProjectVm, ProjectsClient, ProjectsVm } from './../../../api-client';
import { Component, OnInit } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { FieldsetModule } from 'primeng/fieldset';
import { DividerModule } from 'primeng/divider';
import { DependencyInstanceVm, RepositoriesClient, } from '../../../api-client'
import { SelectedDependencyService } from '../dependency-table/selected-dependency.service';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';
import { TreeModule } from 'primeng/tree';
import { TreeNode } from 'primeng/api';
import { CheckboxModule } from 'primeng/checkbox';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-dependency-details-view',
  standalone: true,
  imports: [PanelModule, FieldsetModule, DividerModule, CommonModule, TreeModule, CheckboxModule, FormsModule],
  templateUrl: './dependency-details-view.component.html',
  styleUrl: './dependency-details-view.component.scss'
})
export class DependencyDetailsViewComponent implements OnInit {

  public dependencyInstance: DependencyInstanceVm | null = null;
  public dependencyInstance$: Observable<DependencyInstanceVm | null> | null = null;

  public projectsVm: ProjectsVm | null = null;
  public treeNodes: TreeNode[] | null = [];
  public selectedTreeNode: TreeNode<any> | TreeNode<any>[] | null = null;
  public specificVersionOnly: boolean = true;

  constructor(private readonly selectedDependencyService: SelectedDependencyService, private readonly apiClient: ProjectsClient) { }

  ngOnInit(): void {
    this.dependencyInstance$ = this.selectedDependencyService.getSelectedDependencyInstance();

    this.dependencyInstance$.subscribe({
      next: (dependencyVm) => this.loadProjectsByDependency(dependencyVm),
      error: (error) => this.handleError(error)
    });
  }

  loadProjectsByDependency(dependencyInstance: DependencyInstanceVm | null): void {
    this.dependencyInstance = dependencyInstance;

    const version = this.specificVersionOnly ? dependencyInstance?.version : null;

    this.apiClient.projects_GetByDependency(dependencyInstance?.name, version).subscribe({
      next: (projectsVm) => {
        this.projectsVm = projectsVm;
        this.convertProjectsToTreeNodes(projectsVm);
      },
      error: (error) => this.handleError(error)
    });
  }

  specificVersionOnlyChanged($event: any) {
    if(!this.projectsVm) return;

    this.loadProjectsByDependency(this.dependencyInstance);
  }

  selectedTreeNodeIsRepository(): boolean {
    if(Array.isArray(this.selectedTreeNode) || !this.selectedTreeNode) return false;
    if(!this.selectedTreeNode.leaf) return true;

    return false;
  }

  selectedTreeNodeIsProject(): boolean {
    if(Array.isArray(this.selectedTreeNode) || !this.selectedTreeNode) return false;
    if(this.selectedTreeNode.leaf) return true;

    return false;
  }

  getSelectedTreeNodeDetailsLabel(): string {
    if(this.selectedTreeNodeIsProject()) return 'Project Details';
    if(this.selectedTreeNodeIsRepository()) return 'Repository Details';
    return '';
  }

  treeNodeSelectionChanged($event: TreeNode<any> | TreeNode<any>[] | null) {
      if(!$event || Array.isArray($event)) return;

      if (typeof $event.data === 'string') {

      } else if ($event.data instanceof ProjectVm) {

      }
  }

  private convertProjectsToTreeNodes(projectsVm: ProjectsVm) {
    if (projectsVm.projects && projectsVm.projects.length > 0) {

      this.treeNodes = [];

      const projectsByRepositoryId = this.groupBy(projectsVm.projects, 'repositoryId')

      for (const repositoryId in projectsByRepositoryId) {
        if (Object.prototype.hasOwnProperty.call(projectsByRepositoryId, repositoryId)) {
          const group = projectsByRepositoryId[repositoryId];
          this.treeNodes?.push(this.convertRepositoryProjectsGroupToTreeNode(group))
        }
      }
    }
  }

  private convertRepositoryProjectsGroupToTreeNode(projects: ProjectVm[]): TreeNode {

    const repositoryId = projects[0].id?.toString();
    const repositoryName = projects[0].repositoryName || '';

    const treeNode: TreeNode = {
        key: repositoryId,
        label: repositoryName,
        data: repositoryId,
        expanded: true,
        children: []
    };

    if (projects && projects.length > 0) {
        treeNode.children = projects.map(project => ({
            key: project.id?.toString(),
            label: project.name || '',
            data: project,
            expanded: true,
            leaf: true,
            icon: this.projectTypeToIcon(project.type)
        }));
    }

    return treeNode;
  }

  private projectTypeToIcon(type: string | null | undefined): string {
    if(type === '' || !type) return '';

    if(type.startsWith('Dotnet')) return 'devicon-csharp-plain';
    if(type.startsWith('Angular')) return 'devicon-angular-plain';

    return '';
  }

  private groupBy<T, K extends keyof T>(array: T[], key: K): Record<string, T[]> {
    return array.reduce((result: Record<string, T[]>, currentValue: T) => {
        const propertyValue = currentValue[key] as unknown as string;
        (result[propertyValue] = result[propertyValue] || []).push(currentValue);
        return result;
    }, {});
  }

  private handleError(error: any): void {
    console.error("failed to load dependency details", error);
  }
}
