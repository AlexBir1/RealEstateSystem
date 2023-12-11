import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AgreementModel } from '../models/agreement-item.model';
import { AgreementsModel } from '../models/agreements.model';
import { AuthorizedUser } from '../models/authorized-user.model';
import { ErrorModel } from '../models/error.model';
import { AgreementService } from '../services/agreement.service';
import { LocalStorageService } from '../services/local-storage.service';

@Component({
  selector: 'app-agreement',
  templateUrl: './agreement.component.html',
  styleUrls: ['./agreement.component.css']
})
export class AgreementComponent implements OnInit{
  agreementsModel!: AgreementsModel;
  authorizedUser: AuthorizedUser;
  errorModalContent!: ErrorModel | undefined;
  unexpectedError!: HttpErrorResponse | undefined;
  isLoading: boolean = false;

  constructor(private agreementService: AgreementService, private localStorage: LocalStorageService)
  {    
    this.authorizedUser = localStorage.getAuthorizedUser();
  }

  ngOnInit(): void {
    if(this.authorizedUser.role === "Admin"){
      this.getAgreements();
    }
    else{
      this.getAgreementsByAccountId();
    }
  }

  getAgreements(){
    this.changeLoadingState();
    this.wipeErrors();
    this.agreementService.getAllAgreements().subscribe({
      next: (result) =>{
        if(result.isSuccess){
          this.changeLoadingState();
          
          this.agreementsModel = result.data;
        }
        else{
          this.errorModalContent = new ErrorModel("Something went wrong", result.errors);
        }
      },
      error: (e: HttpErrorResponse)=>{
        this.changeLoadingState();
        this.unexpectedError = e;
      }
    })
  }

  getAgreementsByAccountId(){
    this.changeLoadingState();
    this.wipeErrors();
    this.agreementService.getAllAgreementsByAccountId(this.authorizedUser.userId).subscribe({
      next: (result) =>{
        if(result.isSuccess){
          this.changeLoadingState();

          this.agreementsModel = result.data;
        }
        else{
          this.errorModalContent = new ErrorModel("Something went wrong", result.errors);
        }
      },
      error: (e: HttpErrorResponse)=>{
        this.changeLoadingState();
        this.unexpectedError = e;
      }
    })
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

  updateAgreement(agreementModel: AgreementModel){
    var index = this.agreementsModel.agreements!.findIndex(x=>x.id == agreementModel.id);
    this.agreementsModel.agreements![index] = agreementModel;

    this.agreementsModel = new AgreementsModel(this.agreementsModel.agreements);
  }

  deleteAgreement(agreementModel: AgreementModel){
    var index = this.agreementsModel.agreements!.findIndex(x=>x.id == agreementModel.id);
    this.agreementsModel.agreements!.splice(index, 1);

    this.agreementsModel = new AgreementsModel(this.agreementsModel.agreements);
    
  }
}
