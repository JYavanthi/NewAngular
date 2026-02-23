import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CashCreditComponent } from './cash-credit.component';

describe('CashCreditComponent', () => {
  let component: CashCreditComponent;
  let fixture: ComponentFixture<CashCreditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CashCreditComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CashCreditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
