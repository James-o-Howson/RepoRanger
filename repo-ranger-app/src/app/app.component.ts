import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./components/header/header.component";
import { CommonModule } from '@angular/common';
import { InformationDialogService } from './shared/dialog-boxes/information-dialog/information-dialog.service';
import { ApiModule } from './generated';
import { HttpClientModule } from '@angular/common/http';
import { MatTableModule } from '@angular/material/table';

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
