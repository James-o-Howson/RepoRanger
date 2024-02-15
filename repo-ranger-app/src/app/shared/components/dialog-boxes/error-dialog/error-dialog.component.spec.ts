import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HarnessLoader } from '@angular/cdk/testing';
import { TestbedHarnessEnvironment } from '@angular/cdk/testing/testbed';

import { ErrorDialogComponent, ErrorDialogData } from './error-dialog.component';

import { MaterialModule } from '../../../material/material.module'
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'
import { MatIconHarness } from '@angular/material/icon/testing';

describe('ErrorDialogComponent', () => {
  let component: ErrorDialogComponent;
  let fixture: ComponentFixture<ErrorDialogComponent>;
  let loader: HarnessLoader;

  beforeEach(async () => {
    const matDialogRef = jasmine.createSpyObj('matDialogRef', ['open']);
    const data: ErrorDialogData = { title: 'Test Title', message: 'Test Message', icon: 'error' };

    await TestBed.configureTestingModule({
      declarations: [ErrorDialogComponent],
      providers: [
        { provide: MatDialogRef, useValue: matDialogRef },
        { provide: MAT_DIALOG_DATA, useValue: data }
      ],
      imports: [MaterialModule]
    }).compileComponents();

    fixture = TestBed.createComponent(ErrorDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    loader = TestbedHarnessEnvironment.loader(fixture);

  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display the provided Title, Message and Have Ok Button', (done) => {
    var element = fixture.nativeElement;
    expect(element).toBeTruthy();

    var header = element.querySelector('h3');
    var message = element.querySelector('p');
    var button = element.querySelector('button');

    expect(header.textContent).toEqual('Test Title');
    expect(message.textContent).toEqual('Test Message');
    expect(button.textContent).toEqual('Ok');

    loader.getHarness(MatIconHarness).then((icon) => {
      return icon.getName().then((name) => {
        expect(name).toEqual('error');
      });
    }).finally(done);
  });

});
