import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogClose } from '@angular/material/dialog';
import { MatIcon } from '@angular/material/icon';

export interface ErrorDialogData {
  title: string;
  message: string;
  icon: string;
}

@Component({
  standalone: true,
  selector: 'app-error-dialog',
  templateUrl: './error-dialog.component.html',
  styleUrls: ['./error-dialog.component.scss'],
  imports: [MatIcon, MatDialogClose]
})
export class ErrorDialogComponent {
  public title: string;
  public message: string;
  public icon: string;

  constructor(
    public dialogRef: MatDialogRef<ErrorDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ErrorDialogData,
  ) {
    this.title = data.title;
    this.message = data.message;
    this.icon = data.icon;
  }
}


