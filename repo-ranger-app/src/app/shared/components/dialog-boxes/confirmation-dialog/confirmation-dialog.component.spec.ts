import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HarnessLoader } from '@angular/cdk/testing';
import { TestbedHarnessEnvironment } from '@angular/cdk/testing/testbed';

import { ConfirmationDialogComponent, ConfirmationDialogData } from './confirmation-dialog.component';

import { MaterialModule } from '../../../material/material.module'
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'
import { MatIconHarness } from '@angular/material/icon/testing';

describe('ConfirmationDialogComponent', () => {
  let component: ConfirmationDialogComponent;
  let fixture: ComponentFixture<ConfirmationDialogComponent>;
  let loader: HarnessLoader;

  beforeEach(async () => {
    const matDialogRef = jasmine.createSpyObj('matDialogRef', ['open']);
    const data: ConfirmationDialogData = { title: 'Test Title', message: 'Test Message', icon: 'question_mark', confirmButtonText: "Confirm", declineButtonText: "Cancel" };

    await TestBed.configureTestingModule({
      declarations: [ConfirmationDialogComponent],
      providers: [
        { provide: MatDialogRef, useValue: matDialogRef },
        { provide: MAT_DIALOG_DATA, useValue: data }
      ],
      imports: [MaterialModule]
    }).compileComponents();

    fixture = TestBed.createComponent(ConfirmationDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    loader = TestbedHarnessEnvironment.loader(fixture);

  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display the provided Title, Message and have Confirm and Cancel button', (done) => {
    var element = fixture.nativeElement;
    expect(element).toBeTruthy();

    var header = element.querySelector('h3');
    var message = element.querySelector('p');
    var buttons = element.querySelectorAll('button');

    expect(header.textContent).toEqual('Test Title');
    expect(message.textContent).toEqual('Test Message');
    expect(buttons.length).toEqual(2);

    loader.getHarness(MatIconHarness).then((icon) => {
      return icon.getName().then((name) => {
        expect(name).toEqual('question_mark');
      });
    }).finally(done);
  });

});
