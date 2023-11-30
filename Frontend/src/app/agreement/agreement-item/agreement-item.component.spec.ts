import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AgreementItemComponent } from './agreement-item.component';

describe('AgreementItemComponent', () => {
  let component: AgreementItemComponent;
  let fixture: ComponentFixture<AgreementItemComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AgreementItemComponent]
    });
    fixture = TestBed.createComponent(AgreementItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
