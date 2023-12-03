import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountModel } from 'src/app/models/account.model';
import { APIResponse } from 'src/app/models/api-response';
import { ErrorModel } from 'src/app/models/error.model';
import { AccountService } from 'src/app/services/account.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit{
  account!: AccountModel; 
  changePasswordForm!: FormGroup;
  errors: string[] = [];
  unexpectedError!: HttpErrorResponse | undefined;
  isLoading: boolean = false;
  errorModalContent!: ErrorModel | undefined;

  constructor(private localStorage: LocalStorageService, private accountService: AccountService, private router: Router){
    this.setupChangePasswordForm();
  }

  ngOnInit(){
    var authorizedUser = this.localStorage.getAuthorizedUser();
    if(!authorizedUser)
      this.router.navigateByUrl('/');
    else{
      this.accountService.getAccountById(authorizedUser.userId).subscribe(result => {
        this.account = result.data;
        this.changePasswordForm.controls['id'].setValue(result.data.id);
      });
    }
  }

  setupChangePasswordForm(){
    this.changePasswordForm = new FormGroup({
      id: new FormControl('', Validators.required),
      oldPassword: new FormControl('', Validators.required),
      newPassword: new FormControl('', Validators.required),
      newPasswordConfirm: new FormControl('', Validators.required),
    });
  }

  wipeErrors(){
    this.errorModalContent = undefined;
    this.unexpectedError = undefined;
  }

  changePasswordSubmit(){
    this.wipeErrors();
    this.accountService.changeAccountPassword(this.changePasswordForm.value).subscribe({
      next: (result)=>{
        if(result.isSuccess)
          this.router.navigateByUrl('/Account');
        else
          this.errorModalContent = new ErrorModel("Operation has failed", result.errors);
      },
      error: (e: HttpErrorResponse)=>{
        this.unexpectedError = e;
      }
    });
  }

  changeLoadingState(){
    this.isLoading = !this.isLoading;
  }

  closeErrorModal(){
    this.errorModalContent = undefined;
  }
}
