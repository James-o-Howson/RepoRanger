import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ApiModule } from './generated';
import { HttpClientModule } from '@angular/common/http';
import { MatTableModule } from '@angular/material/table';
import { InformationDialogService } from './core/services/dialogs/information-dialog.service';
import { HeaderComponent } from './shared/components/header/header.component';

@Component({
  standalone: true,
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  imports: [RouterOutlet, HeaderComponent, CommonModule, ApiModule, HttpClientModule, MatTableModule]
})
export class AppComponent {
  title = 'Repo Ranger';

  constructor(private readonly informationDialogService: InformationDialogService) { }

  openSettings(): void {
    this.informationDialogService.show("This will eventually show the settings page", "Settings");
  }

  loginSucceeded(): Boolean {
    return true;
  }
}
