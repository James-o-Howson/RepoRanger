import { TestBed } from '@angular/core/testing';
import { ConfirmationDialogComponent } from './confirmation-dialog.component';
import { ConfirmationDialogService as ConfirmationDialogService } from './confirmation-dialog.service';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { of } from 'rxjs'

describe('ConfirmationDialogService', () => {
  let service: ConfirmationDialogService;
  let dialog: MatDialog;
  let ref: MatDialogRef<ConfirmationDialogService>;

  beforeEach(() => {
    ref = jasmine.createSpyObj<MatDialogRef<ConfirmationDialogService>>(['afterOpened']);
    ref.afterOpened = jasmine.createSpy('afterOpened').and.returnValue(of());

    dialog = jasmine.createSpyObj<MatDialog>(['open']);
    dialog.open = jasmine.createSpy('dialog.open').and.returnValue(ref);

    TestBed.configureTestingModule({
      providers: [
        ConfirmationDialogService,
        { provide: MatDialog, useValue: dialog}
      ]
    });

    service = TestBed.inject(ConfirmationDialogService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('method showConfirmationDialog', () => {

    it('should call dialog.open with default title, icon, confirmButtonText and declineButtonText if they are not supplied', () => {
      var dialogData = { title: 'Confirmation Message', message: 'Supplied Message', icon: 'question_mark', declineButtonText: 'Cancel', confirmButtonText: 'Confirm' }

      service.showConfirmationDialog('Supplied Message');

      expect(dialog.open).toHaveBeenCalledWith(ConfirmationDialogComponent, { data:dialogData });
    });

    it('should call dialog.open with provided message, title, icon, confirmButtonText and declineButtonText overriding their default values', () => {
      var dialogData = { title: 'Supplied Title', message: 'Supplied Message', icon: 'Supplied Icon', declineButtonText: 'Supplied Decline Text', confirmButtonText: 'Supplied Confirm Text' }

      service.showConfirmationDialog('Supplied Message','Supplied Title', "Supplied Confirm Text", "Supplied Decline Text", "Supplied Icon");

      expect(dialog.open).toHaveBeenCalledWith(ConfirmationDialogComponent, { data: dialogData });
    });
  });
});
