import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ServerValidationErrorComponent } from './server-validation-error.component';

describe('ServerValidationErrorComponent', () => {
  let component: ServerValidationErrorComponent;
  let fixture: ComponentFixture<ServerValidationErrorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ServerValidationErrorComponent]
    });
    fixture = TestBed.createComponent(ServerValidationErrorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
