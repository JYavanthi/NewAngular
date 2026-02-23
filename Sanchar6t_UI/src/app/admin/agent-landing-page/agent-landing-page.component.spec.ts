import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AgentLandingPageComponent } from './agent-landing-page.component';

describe('AgentLandingPageComponent', () => {
  let component: AgentLandingPageComponent;
  let fixture: ComponentFixture<AgentLandingPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AgentLandingPageComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AgentLandingPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
