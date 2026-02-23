import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AgentRechargeComponent } from './agent-recharge.component';

describe('AgentRechargeComponent', () => {
  let component: AgentRechargeComponent;
  let fixture: ComponentFixture<AgentRechargeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AgentRechargeComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AgentRechargeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
