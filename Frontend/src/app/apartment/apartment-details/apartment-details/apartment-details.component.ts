import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { ApartmentModel } from 'src/app/models/apartment.model';
import { ApartmentService } from 'src/app/services/apartment.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { environment } from 'src/environments/environment.dev';

@Component({
  selector: 'app-apartment-details',
  templateUrl: './apartment-details.component.html',
  styleUrls: ['./apartment-details.component.css']
})
export class ApartmentDetailsComponent implements OnInit{
  apartmentId!: string;
  apartment!: ApartmentModel;
  apiUrlImagesHttps: string = environment.apiUrlImagesHttps;
  isLoading: boolean = false;
  unexpectedError!: HttpErrorResponse | undefined;
  errors: string[] = [];

  constructor(private activeRoute: ActivatedRoute, private apartmentService: ApartmentService, private localStorage: LocalStorageService, private router: Router){
    this.activeRoute.paramMap.subscribe(x=> this.apartmentId = x.get('apartmentId') as string);
  }

  ngOnInit(): void {
    this.changeLoadingState();
      this.apartmentService.getApartmentById(this.apartmentId).subscribe({
        next: (result) =>
      {
        if(result.isSuccess){
          this.changeLoadingState();
          this.apartment = result.data;
        }
        else{
          this.errors = result.errors;
        }
      },
      error: (e: HttpErrorResponse)=>{
        this.changeLoadingState();
        this.unexpectedError = e;
      }
    });
  }

  wipeErrors(){
    this.unexpectedError = undefined;
  }

  moveIn(apartmentId: string){
    if(!this.localStorage.getAuthorizedUser().userId){
      this.router.navigateByUrl('/Auth');
    }
    else{
      this.router.navigateByUrl('/Apartments/' + apartmentId + '/NewAgreement');
    }
  }

  changeLoadingState(){
    this.isLoading = !this.isLoading;
  }
}
