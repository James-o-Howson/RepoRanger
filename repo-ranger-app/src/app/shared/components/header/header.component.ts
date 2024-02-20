import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { ToolbarModule } from 'primeng/toolbar';

@Component({
  standalone: true,
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
  imports: [ToolbarModule, ButtonModule]
})
export class HeaderComponent {

  constructor() {}
  @Input() title: string = "";
  @Output() openSettings = new EventEmitter<void>();

  onOpenSettings(): void {
    this.openSettings.emit();
  }
}
