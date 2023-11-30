import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { Route, RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { NavbarComponent } from './navbar/navbar.component';
import { ApartmentComponent } from './apartment/apartment.component';
import { ContactsComponent } from './contacts/contacts.component';
import { HomeComponent } from './home/home.component';
import { AuthComponent } from './auth/auth.component';
import { ApartmentDetailsComponent } from './apartment/apartment-details/apartment-details/apartment-details.component';
import { NavbarPopupComponent } from './navbar/navbar-popup/navbar-popup/navbar-popup.component';
import { AccountComponent } from './account/account.component';
import { ChangePasswordComponent } from './account/change-password/change-password/change-password.component';
import { UpdateInfoComponent } from './account/update-info/update-info/update-info.component';
import { ApartmentPhotosComponent } from './apartment/apartment-details/apartment-details/apartment-photos/apartment-photos/apartment-photos.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { OrderComponent } from './order/order.component';
import { AgreementComponent } from './agreement/agreement.component';
import { AgreementItemComponent } from './agreement/agreement-item/agreement-item.component';
import { FormattedDatePipe } from './pipes/convert-date.pipe';
import { FormattedDateTimePipe } from './pipes/convert-datetime.pipe';
import { AuthService } from './services/auth.service';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { CreateApartmentComponent } from './apartment/create-apartment/create-apartment/create-apartment.component';
import { LocalStorageService } from './services/local-storage.service';
import { ApartmentPhotoComponent } from './apartment/apartment-details/apartment-details/apartment-photo/apartment-photo.component';
import { LoadingStateComponent } from './loading-state/loading-state.component';
import { CreateAgreementComponent } from './agreement/create-agreement/create-agreement/create-agreement.component';
import { CreateOrderComponent } from './order/create-order/create-order/create-order.component';
import { ErrorComponent } from './error/error.component';
import { ServerValidationErrorComponent } from './server-validation-error/server-validation-error.component';

const appRoutes: Route[] = [
  { path: '', component: HomeComponent, pathMatch: 'full'},

  { path: 'Contacts', component: ContactsComponent },

  { path: 'Apartments', component: ApartmentComponent },
  { path: 'Apartments/New', component: CreateApartmentComponent },
  { path: 'Apartments/:apartmentId', component: ApartmentDetailsComponent, pathMatch: 'full' },
  { path: 'Apartments/:apartmentId/Edit', component: CreateApartmentComponent },
  { path: 'Apartments/:apartmentId/Photos', component: ApartmentPhotosComponent},
  { path: 'Apartments/:apartmentId/NewAgreement', component: CreateAgreementComponent },
  { path: 'Apartments/:apartmentId/Photos/:photoId', component: ApartmentPhotoComponent },
  { path: 'Apartments/:apartmentId/MainPhoto', component: ApartmentPhotoComponent },

  { path: 'Agreements', component: AgreementComponent },

  { path: 'Auth', component: AuthComponent },

  { path: 'Account', component: AccountComponent},
  { path: 'Account/ChangePassword', component: ChangePasswordComponent},
  { path: 'Account/UpdateInfo', component: UpdateInfoComponent},
  
  { path: 'Orders', component: OrderComponent},
  { path: 'Orders/New', component: CreateOrderComponent},

  { path: 'page-not-found', component: NotFoundComponent},
  
  { path: '**', redirectTo: '/page-not-found'}
];

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    ApartmentComponent,
    ContactsComponent,
    HomeComponent,
    AuthComponent,
    ApartmentDetailsComponent,
    NavbarPopupComponent,
    AccountComponent,
    ChangePasswordComponent,
    UpdateInfoComponent,
    ApartmentPhotosComponent,
    NotFoundComponent,
    OrderComponent,
    AgreementComponent,
    AgreementItemComponent,
    FormattedDatePipe,
    FormattedDateTimePipe,
    CreateApartmentComponent,
    ApartmentPhotoComponent,
    LoadingStateComponent,
    CreateAgreementComponent,
    CreateOrderComponent,
    ErrorComponent,
    ServerValidationErrorComponent,
    
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(appRoutes),
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [AuthService, LocalStorageService],
  bootstrap: [AppComponent]
})
export class AppModule { }
