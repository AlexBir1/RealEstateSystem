import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApartmentSelectComponent } from './apartment-select.component';

describe('ApartmentSelectComponent', () => {
  let component: ApartmentSelectComponent;
  let fixture: ComponentFixture<ApartmentSelectComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ApartmentSelectComponent]
    });
    fixture = TestBed.createComponent(ApartmentSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
