import { TestBed } from '@angular/core/testing';
import { InformationDialogComponent } from './information-dialog.component';
import { InformationDialogService } from './information-dialog.service';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { of } from 'rxjs'

describe('InformationDialogService', () => {
  let service: InformationDialogService;
  let dialog: MatDialog;
  let ref: MatDialogRef<InformationDialogService>;

  beforeEach(() => {
    ref = jasmine.createSpyObj<MatDialogRef<InformationDialogService>>(['afterOpened']);
    ref.afterOpened = jasmine.createSpy('afterOpened').and.returnValue(of());

    dialog = jasmine.createSpyObj<MatDialog>(['open']);
    dialog.open = jasmine.createSpy('dialog.open').and.returnValue(ref);

    TestBed.configureTestingModule({
      providers: [
        InformationDialogService,
        { provide: MatDialog, useValue: dialog}
      ]
    });

    service = TestBed.inject(InformationDialogService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('method showInformationDialog', () => {

    it('should call dialog.open with default title and default icon if title and icon not supplied', () => {
      var dialogData = { title: 'Information Message', message: 'Supplied Message', icon: 'info' }

      service.show('Supplied Message');

      expect(dialog.open).toHaveBeenCalledWith(InformationDialogComponent, { data:dialogData });
    });

    it('should call dialog.open with provided title and default icon if title provided and icon not supplied', () => {
      var dialogData = { title: 'Supplied Title', message: 'Supplied Message', icon: 'info' }

      service.show('Supplied Message','Supplied Title');

      expect(dialog.open).toHaveBeenCalledWith(InformationDialogComponent, { data: dialogData });
    });

    it('should call dialog.open with provided title and provided icon if title and icon provided', () => {
      var dialogData = { title: 'Supplied Title', message: 'Supplied Message', icon: 'home' }

      service.show('Supplied Message', 'Supplied Title','home');

      expect(dialog.open).toHaveBeenCalledWith(InformationDialogComponent, { data: dialogData });
    });

  });
});
