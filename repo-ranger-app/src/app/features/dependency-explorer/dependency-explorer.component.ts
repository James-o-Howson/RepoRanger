import { Component } from '@angular/core';
import { MatExpansionModule } from '@angular/material/expansion';
import { RepositoriesListComponent } from './repositories-list/repositories-list.component';
import { SourcesListComponent } from './sources-list/sources-list.component';
import { RepositoryVm, SourceVm } from '../../generated';
import { MatFormFieldModule } from '@angular/material/form-field';
import { AbstractControl, FormBuilder, FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-dependency-explorer',
  standalone: true,
  templateUrl: './dependency-explorer.component.html',
  styleUrl: './dependency-explorer.component.scss',
  imports: [
    RepositoriesListComponent,
    SourcesListComponent,
    MatExpansionModule,
    MatFormFieldModule,
    ReactiveFormsModule,
  ],
})
export class DependencyExplorerComponent {
  source: SourceVm = {};
  repository: RepositoryVm = {};

  sourcePanelExpanded: boolean = true;
  repositoryPanelExpanded: boolean = false;
  filterForm = this.formBuilder.group({
    source: new FormControl<SourceVm | null | undefined>(null),
  });

  constructor(private readonly formBuilder: FormBuilder) { }

  onSourceSelected(source: SourceVm) {
    this.source = source;
    this.sourcePanelExpanded = false;
    this.repositoryPanelExpanded = true;
  }

  onRepositorySelected(repository: RepositoryVm) {
    this.repository = repository;
    this.repositoryPanelExpanded = false;
  }

  get sourceAbstractControl(): AbstractControl<SourceVm | null | undefined> | null {
    return this.filterForm.get('source');
  }
}
