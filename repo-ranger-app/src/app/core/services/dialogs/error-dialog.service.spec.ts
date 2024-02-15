import { TestBed } from '@angular/core/testing';
import { ErrorDialogComponent } from '../../../shared/components/dialog-boxes/error-dialog/error-dialog.component';
import { ErrorDialogService } from './error-dialog.service';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { of } from 'rxjs'

describe('ErrorDialogService', () => {
  let service: ErrorDialogService;
  let dialog: MatDialog;
  let ref: MatDialogRef<ErrorDialogService>;

  beforeEach(() => {
    ref = jasmine.createSpyObj<MatDialogRef<ErrorDialogService>>(['afterOpened']);
    ref.afterOpened = jasmine.createSpy('afterOpened').and.returnValue(of());

    dialog = jasmine.createSpyObj<MatDialog>(['open']);
    dialog.open = jasmine.createSpy('dialog.open').and.returnValue(ref);

    TestBed.configureTestingModule({
      providers: [
        ErrorDialogService,
        { provide: MatDialog, useValue: dialog}
      ]
    });

    service = TestBed.inject(ErrorDialogService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('method showErrorDialog', () => {

    it('should call dialog.open with default title and default icon if title and icon not supplied', () => {
      var dialogData = { title: 'Error Message', message: 'Supplied Message', icon: 'error' }

      service.show('Supplied Message');

      expect(dialog.open).toHaveBeenCalledWith(ErrorDialogComponent, { data:dialogData });
    });

    it('should call dialog.open with provided title and default icon if title provided and icon not supplied', () => {
      var dialogData = { title: 'Supplied Title', message: 'Supplied Message', icon: 'error' }

      service.show('Supplied Message','Supplied Title');

      expect(dialog.open).toHaveBeenCalledWith(ErrorDialogComponent, { data: dialogData });
    });

    it('should call dialog.open with provided title and provided icon if title and icon provided', () => {
      var dialogData = { title: 'Supplied Title', message: 'Supplied Message', icon: 'home' }

      service.show('Supplied Message', 'Supplied Title','home');

      expect(dialog.open).toHaveBeenCalledWith(ErrorDialogComponent, { data: dialogData });
    });

  });
});
