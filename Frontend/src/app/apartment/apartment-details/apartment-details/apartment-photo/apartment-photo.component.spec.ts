import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApartmentPhotoComponent } from './apartment-photo.component';

describe('ApartmentPhotoComponent', () => {
  let component: ApartmentPhotoComponent;
  let fixture: ComponentFixture<ApartmentPhotoComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ApartmentPhotoComponent]
    });
    fixture = TestBed.createComponent(ApartmentPhotoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
