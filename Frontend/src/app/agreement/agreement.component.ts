import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AgreementsModel } from '../models/agreements.model';
import { AuthorizedUser } from '../models/authorized-user.model';
import { AgreementService } from '../services/agreement.service';
import { LocalStorageService } from '../services/local-storage.service';

@Component({
  selector: 'app-agreement',
  templateUrl: './agreement.component.html',
  styleUrls: ['./agreement.component.css']
})
export class AgreementComponent implements OnInit{
  agreementsModel!: AgreementsModel;
  isLoading: boolean = false;
  authorizedUser: AuthorizedUser;
  errors: string[] = [];
  unexpectedError!: HttpErrorResponse | undefined;

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
        this.changeLoadingState();
        this.agreementsModel = result.data;
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
          this.errors = result.errors;
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
    this.errors = [];
    this.unexpectedError = undefined;
  }
}
