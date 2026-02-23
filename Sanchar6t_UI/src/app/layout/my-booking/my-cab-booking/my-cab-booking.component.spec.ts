import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyCabBookingComponent } from './my-cab-booking.component';

describe('MyCabBookingComponent', () => {
  let component: MyCabBookingComponent;
  let fixture: ComponentFixture<MyCabBookingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MyCabBookingComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MyCabBookingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
