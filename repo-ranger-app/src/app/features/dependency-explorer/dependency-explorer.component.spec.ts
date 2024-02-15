import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DependencyExplorerComponent } from './dependency-explorer.component';

describe('DependencyExplorerComponent', () => {
  let component: DependencyExplorerComponent;
  let fixture: ComponentFixture<DependencyExplorerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DependencyExplorerComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DependencyExplorerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
