import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailedItineraryComponent } from './detailed-itinerary.component';

describe('DetailedItineraryComponent', () => {
  let component: DetailedItineraryComponent;
  let fixture: ComponentFixture<DetailedItineraryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [DetailedItineraryComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DetailedItineraryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
