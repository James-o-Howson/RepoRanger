import { Injectable} from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { InformationDialogComponent, InformationDialogData } from '../../../shared/components/dialog-boxes/information-dialog/information-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class InformationDialogService {
  constructor(private readonly dialog: MatDialog) { }

  show(message: string, title?: string, icon?: string): MatDialogRef<InformationDialogComponent> {
    return this.dialog.open(
      InformationDialogComponent,
      {
        data: {
          title: title ?? 'Information Message',
          message: message,
          icon: icon ?? 'info'
        } as InformationDialogData
      }
    );
  }
}
