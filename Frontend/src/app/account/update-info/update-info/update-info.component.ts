import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountModel } from 'src/app/models/account.model';
import { ErrorModel } from 'src/app/models/error.model';
import { AccountService } from 'src/app/services/account.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';

@Component({
  selector: 'app-update-info',
  templateUrl: './update-info.component.html',
  styleUrls: ['./update-info.component.css']
})
export class UpdateInfoComponent implements OnInit{
  account!: AccountModel; 
  updateInfoForm!: FormGroup;
  errors: string[] = [];
  unexpectedError!: HttpErrorResponse | undefined;
  isLoading: boolean = false;
  errorModalContent!: ErrorModel | undefined;

  constructor(private localStorage: LocalStorageService, private accountService: AccountService, private router: Router){
    this.setupUpdateInfoForm();
  }

  ngOnInit(){
    var authorizedUser = this.localStorage.getAuthorizedUser();
    if(!authorizedUser)
      this.router.navigateByUrl('/');
    else{
      this.changeLoadingState();
      this.accountService.getAccountById(authorizedUser.userId).subscribe(result => {
        this.changeLoadingState();
        this.account = result.data;
        this.updateInfoForm.controls['id'].setValue(result.data.id);
        this.updateInfoForm.controls['fullname'].setValue(result.data.fullname);
        this.updateInfoForm.controls['username'].setValue(result.data.username);
        this.updateInfoForm.controls['email'].setValue(result.data.email);
        this.updateInfoForm.controls['mobilePhone'].setValue(result.data.mobilePhone);
      });
    }
  }

  setupUpdateInfoForm(){
    this.updateInfoForm = new FormGroup({
      id: new FormControl('', Validators.required),
      fullname: new FormControl('', Validators.required),
      username: new FormControl('', Validators.required),
      email: new FormControl('', Validators.required),
      mobilePhone: new FormControl('', Validators.required),
    });
  }

  changeLoadingState(){
    this.isLoading = !this.isLoading;
  }

  wipeErrors(){
    this.errorModalContent = undefined;
    this.unexpectedError = undefined;
  }

  updateInfoSubmit(){
    this.wipeErrors();
    this.accountService.updateAccount(this.updateInfoForm.value).subscribe({
      next:(result)=>{
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

  closeErrorModal(){
    this.errorModalContent = undefined;
  }
}
