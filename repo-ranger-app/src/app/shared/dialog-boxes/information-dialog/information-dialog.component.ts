import { Component, Inject } from '@angular/core';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA, MatDialogClose } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

export interface InformationDialogData {
  title: string;
  message: string;
  icon: string;
}

@Component({
  standalone: true,
  selector: 'app-information-dialog',
  templateUrl: './information-dialog.component.html',
  styleUrls: ['./information-dialog.component.scss'],
  imports: [
    MatIconModule, 
    MatDialogClose, 
    MatButtonModule,
    MatDialogModule]
})
export class InformationDialogComponent {
  public title: string;
  public message: string;
  public icon: string;

  constructor(
    public dialogRef: MatDialogRef<InformationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: InformationDialogData,
  ) {
    this.title = data.title;
    this.message = data.message;
    this.icon = data.icon;
  }
}