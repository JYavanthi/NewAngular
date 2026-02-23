import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BusAddPackageComponent } from './bus-add-package.component';

describe('BusAddPackageComponent', () => {
  let component: BusAddPackageComponent;
  let fixture: ComponentFixture<BusAddPackageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BusAddPackageComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BusAddPackageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
