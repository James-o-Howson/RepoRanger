import { Injectable} from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ErrorDialogComponent, ErrorDialogData } from '../../../shared/components/dialog-boxes/error-dialog/error-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class ErrorDialogService {
  constructor(private dialog: MatDialog) { }

  show(message: string, title?: string, icon?: string): MatDialogRef<ErrorDialogComponent> {
    return this.dialog.open(
      ErrorDialogComponent,
      {
        data: {
          title: title ?? 'Error Message',
          message: message,
          icon: icon ?? 'error'
        } as ErrorDialogData
      }
    );
  }
}
