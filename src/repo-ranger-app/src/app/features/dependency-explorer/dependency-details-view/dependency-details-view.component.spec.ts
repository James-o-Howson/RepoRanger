import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DependencyDetailsViewComponent } from './dependency-details-view.component';

describe('DependencyDetailsViewComponent', () => {
  let component: DependencyDetailsViewComponent;
  let fixture: ComponentFixture<DependencyDetailsViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DependencyDetailsViewComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DependencyDetailsViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
