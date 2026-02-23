import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PackageImageDetailsComponent } from './package-image-details.component';

describe('PackageImageDetailsComponent', () => {
  let component: PackageImageDetailsComponent;
  let fixture: ComponentFixture<PackageImageDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PackageImageDetailsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PackageImageDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
