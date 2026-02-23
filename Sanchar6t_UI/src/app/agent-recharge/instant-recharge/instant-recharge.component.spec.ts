import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InstantRechargeComponent } from './instant-recharge.component';

describe('InstantRechargeComponent', () => {
  let component: InstantRechargeComponent;
  let fixture: ComponentFixture<InstantRechargeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [InstantRechargeComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(InstantRechargeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
