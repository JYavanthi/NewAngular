import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyBusBookingComponent } from './my-bus-booking.component';

describe('MyBusBookingComponent', () => {
  let component: MyBusBookingComponent;
  let fixture: ComponentFixture<MyBusBookingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MyBusBookingComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MyBusBookingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
