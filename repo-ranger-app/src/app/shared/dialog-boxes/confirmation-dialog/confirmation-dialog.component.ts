import { Component, Inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogClose, MatDialogModule } from '@angular/material/dialog';
import { MatIcon, MatIconModule } from '@angular/material/icon';

export interface ConfirmationDialogData {
  title: string;
  message: string;
  icon: string;
  confirmButtonText: string;
  declineButtonText: string;
}

@Component({
  standalone: true,
  selector: 'app-confirmation-dialog',
  templateUrl: './confirmation-dialog.component.html',
  styleUrls: ['./confirmation-dialog.component.scss'],
  imports: [
    MatIconModule,
    MatDialogClose,
    MatButtonModule,
    MatDialogModule]
})
export class ConfirmationDialogComponent {
  public title: string;
  public message: string;
  public icon: string;
  public confirmButtonText: string;
  public declineButtonText: string;

  constructor(
    public dialogRef: MatDialogRef<ConfirmationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ConfirmationDialogData,
  ) {
    this.title = data.title;
    this.message = data.message;
    this.icon = data.icon;
    this.confirmButtonText = data.confirmButtonText;
    this.declineButtonText = data.declineButtonText;
  }

  decline() {
    this.dialogRef.close(false);
  }

  confirm() {
    this.dialogRef.close(true);
  }
}


