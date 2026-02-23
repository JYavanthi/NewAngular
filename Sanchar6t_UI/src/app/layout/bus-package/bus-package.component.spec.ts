import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BusPackageComponent } from './bus-package.component';

describe('BusPackageComponent', () => {
  let component: BusPackageComponent;
  let fixture: ComponentFixture<BusPackageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BusPackageComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BusPackageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
