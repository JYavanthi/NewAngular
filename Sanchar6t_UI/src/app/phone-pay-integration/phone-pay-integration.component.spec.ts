import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PhonePayIntegrationComponent } from './phone-pay-integration.component';

describe('PhonePayIntegrationComponent', () => {
  let component: PhonePayIntegrationComponent;
  let fixture: ComponentFixture<PhonePayIntegrationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PhonePayIntegrationComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PhonePayIntegrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
