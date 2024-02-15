import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { SourceVm, SourcesService, SourcesVm } from '../../../generated';
import { ErrorHandlerService } from '../../../core/error-handler.service';
import { MatSelectModule } from '@angular/material/select';
import { MatLabel } from '@angular/material/form-field';

@Component({
  standalone: true,
  selector: 'app-sources-list',
  templateUrl: './sources-list.component.html',
  styleUrl: './sources-list.component.scss',
  imports: [MatSelectModule, MatLabel],
})
export class SourcesListComponent implements OnInit {
  @Output() sourceSelected = new EventEmitter<SourceVm>();

  sources: Array<SourceVm> | null | undefined = [];
  selectedSource: SourceVm | null | undefined;

  constructor(
    private readonly sourcesService: SourcesService,
    private readonly errorHandlerService: ErrorHandlerService
  ) {}

  ngOnInit(): void {
    this.sourcesService.apiSourcesGet().subscribe({
      next: (sourcesVm) => this.handleApiSourcesGetSuccess(sourcesVm),
      error: (error) => this.errorHandlerService.handleError(error),
    });
  }

  handleApiSourcesGetSuccess(sourcesVm: SourcesVm): void {
    console.log(sourcesVm);
    this.sources = sourcesVm.sources;
  }

  sourceChanged() {
    if(!this.selectedSource) return;
    this.sourceSelected.emit(this.selectedSource);
  }
}
