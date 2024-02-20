import { Component, OnInit } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { FieldsetModule } from 'primeng/fieldset';
import { DividerModule } from 'primeng/divider';
import { DependencyVm } from '../../../generated';
import { SelectedDependencyService } from '../dependency-table/selected-dependency.service';
import { Observable } from 'rxjs';
import { CommonModule, NgIf } from '@angular/common';

@Component({
  selector: 'app-dependency-details-view',
  standalone: true,
  imports: [PanelModule, FieldsetModule, DividerModule, CommonModule],
  templateUrl: './dependency-details-view.component.html',
  styleUrl: './dependency-details-view.component.scss'
})
export class DependencyDetailsViewComponent implements OnInit {

  public dependency$: Observable<DependencyVm | null> | null = null;

  constructor(private readonly selectedDependencyService: SelectedDependencyService) { }

  ngOnInit(): void {
    this.dependency$ = this.selectedDependencyService.getSelectedDependency();
  }
}
