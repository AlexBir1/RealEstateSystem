import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AgreementModel } from 'src/app/models/agreement-item.model';
import { ApartmentModel } from 'src/app/models/apartment.model';
import { AuthorizedUser } from 'src/app/models/authorized-user.model';
import { AgreementService } from 'src/app/services/agreement.service';
import { ApartmentService } from 'src/app/services/apartment.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';

@Component({
  selector: 'app-create-agreement',
  templateUrl: './create-agreement.component.html',
  styleUrls: ['./create-agreement.component.css']
})
export class CreateAgreementComponent implements OnInit{
  apartmentId!: string;
  agreement!: AgreementModel;
  apartment!: ApartmentModel;
  isLoading: boolean = false;
  authorizedUser: AuthorizedUser;
  agreementForm!: FormGroup;
  errors: string[] = [];
  unexpectedError!: HttpErrorResponse | undefined;
  
  constructor(
    private activeRoute: ActivatedRoute, 
    private router: Router, 
    private apartmentService: ApartmentService, 
    private localStorage: LocalStorageService, 
    private agreementService: AgreementService
  ){
    this.setupMonthField();
    this.authorizedUser = localStorage.getAuthorizedUser();
    this.activeRoute.paramMap.subscribe(x=> this.apartmentId = x.get('apartmentId') as string);
  }

  ngOnInit(): void {
    this.changeLoadingState();
    this.wipeErrors();
    this.apartmentService.getApartmentById(this.apartmentId).subscribe({
      next: (result) =>{
      this.changeLoadingState();
      this.apartment = result.data;
      this.agreement = new AgreementModel(result.data, this.authorizedUser.userId, 1);
    },
    error: (e: HttpErrorResponse)=>{
      this.unexpectedError = e;
    }
  })
  }

  onMonthsFieldChange(){
    if(!this.agreementForm.controls['months'].value){
      this.agreement.paymentsToMakeCount = 1;
      this.agreement.monthCountBeforeExpiration = 1;
    }
    else{
      this.agreement.paymentsToMakeCount = this.agreementForm.controls['months'].value;
      this.agreement.monthCountBeforeExpiration = this.agreementForm.controls['months'].value;
    }
  }

  setupMonthField(){
    this.agreementForm = new FormGroup({
      months: new FormControl(1, Validators.required),
    });
  }

  createAgreement(){
    this.changeLoadingState();
    this.wipeErrors();
    var newAgreement: AgreementModel = this.agreement;
    this.agreementService.createAgreement(newAgreement).subscribe({
      next: (result) =>{
      this.changeLoadingState();
      if(result.isSuccess){
        this.changeLoadingState();
        this.router.navigateByUrl('/Agreements');
      }
      else{
        this.errors = result.errors;
      }
    },
    error: (e: HttpErrorResponse)=>{
      this.unexpectedError = e;
    }
    });
  }

  wipeErrors(){
    this.errors = [];
    this.unexpectedError = undefined;
  }

  changeLoadingState(){
    this.isLoading = !this.isLoading;
  }
}
