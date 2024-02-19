import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  standalone: true,
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
  imports: []
})
export class HeaderComponent {

  constructor() {}
  @Input() title: string = "";
  @Output() openSettings = new EventEmitter<void>();

  onOpenSettings(): void {
    this.openSettings.emit();
  }
}
