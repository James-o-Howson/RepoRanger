import { ProjectVm, RepositoryAggregateVm, RepositoryAggregatesVm } from './../../../api-client';
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

  public dependencyInstance$: Observable<DependencyInstanceVm | null> | null = null;
  public repositoryAggregates: RepositoryAggregatesVm | null = null;
  public repositoryTreeNodes: TreeNode[] | null = [];
  public selectedTreeNode: TreeNode<any> | TreeNode<any>[] | null = null;
  public specificVersionOnly: boolean = true;

  constructor(private readonly selectedDependencyService: SelectedDependencyService, private readonly apiClient: RepositoriesClient) { }

  ngOnInit(): void {
    this.dependencyInstance$ = this.selectedDependencyService.getSelectedDependencyInstance();

    this.dependencyInstance$.subscribe({
      next: (dependencyVm) => this.handleLoadSelectedDependencySucceess(dependencyVm),
      error: (error) => this.handleError(error)
    });
  }

  handleLoadSelectedDependencySucceess(dependencyInstance: DependencyInstanceVm | null): void {
    this.apiClient.repositories_GetByDependencyName(dependencyInstance?.name).subscribe({
      next: (depdnencyDetailVm) => this.handleGetDependencyDetailSuccess(depdnencyDetailVm),
      error: (error) => this.handleError(error)
    });
  }

  handleGetDependencyDetailSuccess(repositoryAggregates: RepositoryAggregatesVm): void {
    this.repositoryAggregates = repositoryAggregates;

    this.convertRepositoriesToTreeNodes(repositoryAggregates);
  }

  handleError(error: any): void {
    console.error("failed to load selected dependency", error);
  }

  specificVersionOnlyChanged($event: any) {
    if(!this.repositoryAggregates) return;
    this.convertRepositoriesToTreeNodes(this.repositoryAggregates);
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
    if(this.selectedTreeNodeIsRepository()) return 'Project Details';
    return '';
  }

  private convertRepositoriesToTreeNodes(repositoryAggregates: RepositoryAggregatesVm) {
    if (repositoryAggregates.repositoryAggregates && repositoryAggregates.repositoryAggregates.length > 0) {
      this.repositoryTreeNodes = repositoryAggregates.repositoryAggregates.map(repo => this.convertRepositoryToTreeNode(repo));
    }
  }

  private convertRepositoryToTreeNode(repositoryAggregate: RepositoryAggregateVm): TreeNode {
    const treeNode: TreeNode = {
        key: repositoryAggregate.id?.toString(),
        label: repositoryAggregate.name || '',
        data: repositoryAggregate,
        expanded: true,
        children: []
    };

    if (repositoryAggregate.projects && repositoryAggregate.projects.length > 0) {
        treeNode.children = repositoryAggregate.projects.map(project => ({
            key: project.id?.toString(),
            label: this.getProjectTreeNodeLabel(project),
            data: project,
            expanded: true,
            leaf: true,
            icon: this.versionToIcon(project.version)
        }));
    }

    return treeNode;
  }

  private getProjectTreeNodeLabel(project: ProjectVm): string {
    if(this.specificVersionOnly) {
      return project.name || '';
    }
    else {
      return `${project.name || ''} (${project.version})`;
    }
  }

  private versionToIcon(version: string | null | undefined): string {
    if(version === '' || !version) return '';

    if(version.startsWith('Dotnet')) return 'devicon-csharp-plain';
    if(version.startsWith('Angular')) return 'devicon-angular-plain';

    return '';
  }
}
