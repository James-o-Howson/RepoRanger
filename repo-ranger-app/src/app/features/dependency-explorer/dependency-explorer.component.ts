import { DependencyVm } from './../../generated/model/dependency-vm';
import { Component } from '@angular/core';
import { RepositoryVm, SourceVm } from '../../generated';
import { AbstractControl, FormBuilder, FormControl, ReactiveFormsModule } from '@angular/forms';
import { DependencyTableComponent } from './dependency-table/dependency-table.component';

@Component({
  selector: 'app-dependency-explorer',
  standalone: true,
  templateUrl: './dependency-explorer.component.html',
  styleUrl: './dependency-explorer.component.scss',
  imports: [DependencyTableComponent],
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
