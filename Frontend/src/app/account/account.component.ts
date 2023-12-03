import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountModel } from '../models/account.model';
import { ErrorModel } from '../models/error.model';
import { AccountService } from '../services/account.service';
import { LocalStorageService } from '../services/local-storage.service';
import { makeJWTHeader } from '../utilities/make-jwt-header';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit{
  account!: AccountModel; 
  isLoading: boolean = false;
  unexpectedError!: HttpErrorResponse | undefined;
  errorModalContent!: ErrorModel | undefined;

  constructor(private localStorage: LocalStorageService, private accountService: AccountService, private router: Router){

  }
  ngOnInit(){
    var authorizedUser = this.localStorage.getAuthorizedUser();
    if(!authorizedUser)
      this.router.navigateByUrl('/');
    else{
      this.changeLoadingState();
      this.wipeErrors();
      this.accountService.getAccountById(authorizedUser.userId).subscribe({
        next: (result) => {
          this.changeLoadingState();
          if(result.isSuccess){
          this.account = result.data;
          }
          else{
            this.errorModalContent = new ErrorModel("Operation has failed", result.errors);
          }
        },
        error: (e: HttpErrorResponse)=>{
          this.changeLoadingState();
          this.unexpectedError = e;
        }
      });
    }
  }

  wipeErrors(){
    this.errorModalContent = undefined;
    this.unexpectedError = undefined;
  }

  changeLoadingState(){
    this.isLoading = !this.isLoading;
  }

  closeErrorModal(){
    this.errorModalContent = undefined;
  }
}
