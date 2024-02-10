import { Injectable} from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ConfirmationDialogComponent, ConfirmationDialogData } from './confirmation-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class ConfirmationDialogService {
  constructor(private readonly dialog: MatDialog) { }

  showConfirmationDialog(message: string, title?: string, confirmButtonText?: string, declineButtonText?: string, icon?: string): MatDialogRef<ConfirmationDialogComponent> {
    return this.dialog.open(
      ConfirmationDialogComponent,
      {
        data: {
          title: title ?? 'Confirmation Message',
          message: message,
          icon: icon ?? 'question_mark',
          declineButtonText: declineButtonText ?? "Cancel",
          confirmButtonText: confirmButtonText ?? "Confirm"
        } as ConfirmationDialogData
      }
    );
  }
}
