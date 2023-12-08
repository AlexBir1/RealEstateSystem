import { HttpErrorResponse } from '@angular/common/http';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AgreementModel } from 'src/app/models/agreement-item.model';
import { ErrorModel } from 'src/app/models/error.model';
import { AgreementService } from 'src/app/services/agreement.service';

@Component({
  selector: 'app-agreement-item',
  templateUrl: './agreement-item.component.html',
  styleUrls: ['./agreement-item.component.css']
})
export class AgreementItemComponent {
  @Input() agreement!: AgreementModel; 
  isExpanded: boolean = false;
  isPayment: boolean = false;
  paymentForm!: FormGroup;

  errorModalContent!: ErrorModel | undefined;
  unexpectedError!: HttpErrorResponse | undefined;
  isLoading: boolean = false;

  @Output() updateAgreementEvent: EventEmitter<AgreementModel> = new EventEmitter<AgreementModel>();
  @Output() deleteAgreementEvent: EventEmitter<AgreementModel> = new EventEmitter<AgreementModel>();

  constructor(private agreementService: AgreementService, private router: Router)
  {    
    this.setupMonthField();
  }

  setupMonthField(){
    this.paymentForm = new FormGroup({
      paymentsCount: new FormControl(1, Validators.required),
    });
  }

  onExpand(){
    this.isExpanded = !this.isExpanded;
  }

  changeIsPayment(){
    this.isPayment = !this.isPayment;
  }

  onPaymentsFieldChange(){
    if(this.paymentForm.controls['paymentsCount'].value && this.paymentForm.controls['paymentsCount'].value > this.agreement.paymentsToMakeCount)
    this.paymentForm.controls['paymentsCount'].setValue(this.agreement.paymentsToMakeCount);
  }

  commitPayment(){
    var paymentsCount = this.paymentForm.controls['paymentsCount'].value;
    this.agreement.paymentsToMakeCount += -paymentsCount;
    this.agreement.paymentsMadeCount += paymentsCount;

    this.changeLoadingState();

    this.agreementService.updateAgreement(this.agreement).subscribe({
      next: (r) =>{
        this.changeLoadingState();
        if(r.isSuccess){
          this.updateAgreementEvent.emit(r.data);
        }
        else{
          this.errorModalContent = new ErrorModel("Operation has failed", r.errors);
        }
      },
      error: (e: HttpErrorResponse)=>{
        this.changeLoadingState();
        this.unexpectedError = e;
      }
    });
  }

  changeLoadingState(){
    this.isLoading = !this.isLoading;
  }

  wipeErrors(){
    this.errorModalContent = undefined;
    this.unexpectedError = undefined;
  }

  closeErrorModal(){
    this.errorModalContent = undefined;
  }

  deletePayment(){
    this.changeLoadingState();
    this.agreementService.deleteAgreement(this.agreement.id).subscribe({
      next: (r) =>{
        this.changeLoadingState();
        if(r.isSuccess){
          this.deleteAgreementEvent.emit(r.data);
        }
        else{
          this.errorModalContent = new ErrorModel("Operation has failed", r.errors);
        }
      },
      error: (e: HttpErrorResponse)=>{
        this.changeLoadingState();
        this.unexpectedError = e;
      }
    })
  }
}
