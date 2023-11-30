import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApartmentPhotosComponent } from './apartment-photos.component';

describe('ApartmentPhotosComponent', () => {
  let component: ApartmentPhotosComponent;
  let fixture: ComponentFixture<ApartmentPhotosComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ApartmentPhotosComponent]
    });
    fixture = TestBed.createComponent(ApartmentPhotosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
