import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BusDashboardComponent } from './bus-dashboard.component';

describe('BusDashboardComponent', () => {
  let component: BusDashboardComponent;
  let fixture: ComponentFixture<BusDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BusDashboardComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BusDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
