import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ErrorModel } from './models/error.model';
import { AccountService } from './services/account.service';
import { AgreementService } from './services/agreement.service';
import { ApartmentService } from './services/apartment.service';
import { AuthService } from './services/auth.service';
import { CallService } from './services/call.service';
import { ContactsService } from './services/contacts.service';
import { LocalStorageService } from './services/local-storage.service';
import { OrderService } from './services/order.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [
    AccountService, 
    AgreementService, 
    ApartmentService, 
    CallService, 
    ContactsService, 
    OrderService
  ]
})
export class AppComponent implements OnInit{

  unexpectedError!: HttpErrorResponse | undefined;
  errorModalContent!: ErrorModel | undefined;
  
  constructor(private localStorage: LocalStorageService, private authService: AuthService, private router: Router) {}

  ngOnInit() {
    var authorizedUser = this.localStorage.getAuthorizedUser();
    if(authorizedUser){
      var date = Date.now();
      var expirationDate = new Date(authorizedUser.tokenExpirationDate).getTime();

      if(authorizedUser.keepAuthorized){
        if(expirationDate < date)
          this.authService.refreshAuthToken(authorizedUser).subscribe({next:(result) =>{

          },
        error: (e) => {
          this.authService.updateAuthorizedUserInTheService(authorizedUser);
        }});
        else
          this.authService.updateAuthorizedUserInTheService(authorizedUser);
      }
      else{
        this.localStorage.setAuthorizedUser({userId: '', jwt: '', role: '', keepAuthorized: false, tokenExpirationDate: new Date()})
        this.router.navigateByUrl('/');
      }
    }
  }
  title = 'ApartmentReviwer';

  wipeErrors(){
    this.errorModalContent = undefined;
    this.unexpectedError = undefined;
  }

  closeErrorModal(){
    this.errorModalContent = undefined;
  }
}
