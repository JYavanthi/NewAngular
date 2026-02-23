import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowMyTicketComponent } from './show-my-ticket.component';

describe('ShowMyTicketComponent', () => {
  let component: ShowMyTicketComponent;
  let fixture: ComponentFixture<ShowMyTicketComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ShowMyTicketComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ShowMyTicketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
