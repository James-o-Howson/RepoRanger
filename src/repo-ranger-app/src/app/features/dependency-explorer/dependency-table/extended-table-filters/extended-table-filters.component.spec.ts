import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExtendedTableFiltersComponent } from './extended-table-filters.component';

describe('ExtendedTableFiltersComponent', () => {
  let component: ExtendedTableFiltersComponent;
  let fixture: ComponentFixture<ExtendedTableFiltersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExtendedTableFiltersComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ExtendedTableFiltersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
