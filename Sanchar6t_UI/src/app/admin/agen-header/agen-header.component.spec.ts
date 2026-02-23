import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AgenHeaderComponent } from './agen-header.component';

describe('AgenHeaderComponent', () => {
  let component: AgenHeaderComponent;
  let fixture: ComponentFixture<AgenHeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AgenHeaderComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AgenHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
