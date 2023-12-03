import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ErrorModel } from '../models/error.model';
import { AuthService } from '../services/auth.service';
import { LocalStorageService } from '../services/local-storage.service';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css'],
  providers: [LocalStorageService]
})
export class AuthComponent {
  signUpForm!: FormGroup;
  logInForm!: FormGroup;
  errors: string[] = [];

  isLoading: boolean = false;
  unexpectedError!: HttpErrorResponse | undefined;
  errorModalContent!: ErrorModel | undefined;

  constructor(private authService: AuthService, private localStorage: LocalStorageService, private router: Router){
    this.setupLogInForm();
    this.setupSignUpForm();
  }

  setupLogInForm(){
    this.logInForm = new FormGroup({
      userIdentifier: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
      keepAuthorized: new FormControl(false, Validators.required),
    });
  }

  setupSignUpForm(){
    this.signUpForm = new FormGroup({
      username: new FormControl('', Validators.required),
      fullName: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required, Validators.email]),
      mobilePhone: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
      passwordConfirm: new FormControl('', Validators.required),
      keepAuthorized: new FormControl(false, Validators.required),
    });
  }

  changeForm(){
    this.errors = [];
    var forms = document.querySelectorAll('.auth_form');
    if(forms[0].classList.contains('active')){
      forms[0].classList.remove('active');
      forms[1].classList.add('active');

    }
    else{
      forms[0].classList.add('active');
      forms[1].classList.remove('active');
    }
  }

  onLogInSubmit(){
    this.errors = [];
    this.changeLoadingState();
    this.wipeErrors();
    this.authService.signIn(this.logInForm!.value).subscribe({
      next: (res)=>{
        this.changeLoadingState();
        if(res.isSuccess){
          this.localStorage.setAuthorizedUser(res.data);
          this.router.navigateByUrl('/');
        }
        else
          this.errorModalContent = new ErrorModel("Operation has failed", res.errors);
      },
      error: (e: HttpErrorResponse)=>{
        this.changeLoadingState();
        this.unexpectedError = e;
      }
  });
  }

  onSignUpSubmit(){
    this.errors = [];
    this.changeLoadingState();
    this.wipeErrors();
    this.authService.signUp(this.signUpForm!.value).subscribe({
      next: (res)=>{
        this.changeLoadingState();
        if(res.isSuccess){
          this.localStorage.setAuthorizedUser(res.data);
          this.router.navigateByUrl('/');
        }
        else
        this.errorModalContent = new ErrorModel("Operation has failed", res.errors);
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
    this.unexpectedError = undefined;
  }

  closeErrorModal(){
    this.errorModalContent = undefined;
  }
}
