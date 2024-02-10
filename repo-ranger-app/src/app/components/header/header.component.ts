import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button'

@Component({
  standalone: true,
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss',
  imports: [MatToolbarModule, MatIconModule, MatButtonModule]
})
export class HeaderComponent {

  constructor() {}
  @Input() title: string = "";
  @Output() openSettings = new EventEmitter<void>();

  onOpenSettings(): void {
    this.openSettings.emit();
  }
}
