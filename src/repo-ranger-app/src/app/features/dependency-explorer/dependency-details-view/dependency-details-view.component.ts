import { Component, OnInit } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { FieldsetModule } from 'primeng/fieldset';
import { DividerModule } from 'primeng/divider';
import { DependencyInstanceDetailVm, DependencyInstanceVm, DependencyInstancesClient, ProjectDetailVm, RepositoryDetailVm } from '../../../api-client'
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

  public dependencyInstance$: Observable<DependencyInstanceDetailVm | null> | null = null;
  public dependencyDetail: DependencyInstanceDetailVm | null = null;
  public repositoryTreeNodes: TreeNode[] | null = [];
  public selectedTreeNode: TreeNode<any> | TreeNode<any>[] | null = null;
  public specificVersionOnly: boolean = true;

  constructor(private readonly selectedDependencyService: SelectedDependencyService, private readonly apiClient: DependencyInstancesClient) { }

  ngOnInit(): void {
    this.dependencyInstance$ = this.selectedDependencyService.getSelectedDependencyInstance();

    this.dependencyInstance$.subscribe({
      next: (dependencyVm) => this.handleLoadSelectedDependencySucceess(dependencyVm),
      error: (error) => this.handleError(error)
    });
  }

  handleLoadSelectedDependencySucceess(dependencyVm: DependencyInstanceVm | null): void {
    this.apiClient.dependencyInstances_Get(dependencyVm?.id).subscribe({
      next: (depdnencyDetailVm) => this.handleGetDependencyDetailSuccess(depdnencyDetailVm),
      error: (error) => this.handleError(error)
    });
  }

  handleGetDependencyDetailSuccess(depdnencyDetailVm: DependencyInstanceDetailVm): void {
    this.dependencyDetail = depdnencyDetailVm;

    this.convertRepositoriesToTreeNodes(depdnencyDetailVm);
  }

  handleError(error: any): void {
    console.error("failed to load selected dependency", error);
  }

  specificVersionOnlyChanged($event: any) {
    if(!this.dependencyDetail) return;
    this.convertRepositoriesToTreeNodes(this.dependencyDetail);
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

  private convertRepositoriesToTreeNodes(depdnencyDetailVm: DependencyInstanceDetailVm) {
    if (depdnencyDetailVm.repositories && depdnencyDetailVm.repositories.length > 0) {
      this.repositoryTreeNodes = depdnencyDetailVm.repositories.map(repo => this.convertRepositoryToTreeNode(repo));
    }
  }

  private convertRepositoryToTreeNode(repository: RepositoryDetailVm): TreeNode {
    const treeNode: TreeNode = {
        key: repository.id?.toString(),
        label: repository.name || '',
        data: repository,
        expanded: true,
        children: []
    };

    if (repository.projects && repository.projects.length > 0) {
        treeNode.children = repository.projects.map(project => ({
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

  private getProjectTreeNodeLabel(project: ProjectDetailVm): string {
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
