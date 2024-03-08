import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { HeaderComponent } from './shared/components/header/header.component';

@Component({
  standalone: true,
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  imports: [RouterOutlet, HeaderComponent, CommonModule, HttpClientModule]
})
export class AppComponent {
  title = 'Repo Ranger';

  constructor() { }

  openSettings(): void {
  }

  loginSucceeded(): Boolean {
    return true;
  }
}
