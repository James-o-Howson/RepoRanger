import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DependencyTableComponent } from './dependency-table.component';

describe('DependencyTableComponent', () => {
  let component: DependencyTableComponent;
  let fixture: ComponentFixture<DependencyTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DependencyTableComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DependencyTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
