import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./components/header/header.component";
import { CommonModule } from '@angular/common';
import { InformationDialogService } from './shared/dialog-boxes/information-dialog/information-dialog.service';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  imports: [RouterOutlet, HeaderComponent, CommonModule]
})
export class AppComponent {
  title = 'Repo Ranger';

  constructor(private readonly informationDialogService: InformationDialogService) {
    
  }

  openSettings(): void {
    this.informationDialogService.showInformationDialog("This will eventually show the settings page", "Settings");
  }

  loginSucceeded(): Boolean {
    return true;
  }
}
