import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ApiModule } from './generated';
import { HttpClientModule } from '@angular/common/http';
import { HeaderComponent } from './shared/components/header/header.component';

@Component({
  standalone: true,
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  imports: [RouterOutlet, HeaderComponent, CommonModule, ApiModule, HttpClientModule]
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
