import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeTravelDateComponent } from './change-travel-date.component';

describe('ChangeTravelDateComponent', () => {
  let component: ChangeTravelDateComponent;
  let fixture: ComponentFixture<ChangeTravelDateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ChangeTravelDateComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ChangeTravelDateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
