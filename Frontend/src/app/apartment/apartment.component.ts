import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment.dev';
import { ApartmentModel } from '../models/apartment.model';
import { AuthorizedUser } from '../models/authorized-user.model';
import { ApartmentService } from '../services/apartment.service';
import { LocalStorageService } from '../services/local-storage.service';

@Component({
  selector: 'app-apartment',
  templateUrl: './apartment.component.html',
  styleUrls: ['./apartment.component.css']
})
export class ApartmentComponent implements OnInit {
  apartments: ApartmentModel[] = [];
  authorizedUser: AuthorizedUser;
  apiUrlImagesHttps: string = environment.apiUrlImagesHttps;
  isLoading: boolean = false;
  unexpectedError!: HttpErrorResponse | undefined;
  errors: string[] = [];


  constructor(private apartmentService: ApartmentService, private localStorage: LocalStorageService)
  {
    this.authorizedUser = localStorage.getAuthorizedUser();
  }
  

  ngOnInit(): void {
    this.changeLoadingState();
    this.wipeErrors();
    this.apartmentService.getAllApartments().subscribe({
      next: (result) => 
      {
        if(result.isSuccess){
          this.changeLoadingState();
          this.apartments = result.data;
        }
        else{
          this.errors = result.errors;
        }
    }, 
      error: (e: HttpErrorResponse)=>{
      this.changeLoadingState();
      this.unexpectedError = e;
    }});
  }
  
  changeLoadingState(){
    this.isLoading = !this.isLoading;
  }

  wipeErrors(){
    this.errors = [];
    this.unexpectedError = undefined;
  }
}
