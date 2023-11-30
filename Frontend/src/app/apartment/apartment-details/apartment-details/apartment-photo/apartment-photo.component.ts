import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ApartmentPhotoModel } from 'src/app/models/apartment-photo.model';
import { ApartmentModel } from 'src/app/models/apartment.model';
import { AuthorizedUser } from 'src/app/models/authorized-user.model';
import { ApartmentService } from 'src/app/services/apartment.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { environment } from 'src/environments/environment.dev';

@Component({
  selector: 'app-apartment-photo',
  templateUrl: './apartment-photo.component.html',
  styleUrls: ['./apartment-photo.component.css']
})
export class ApartmentPhotoComponent implements OnInit{
  apartmentId!: string;
  photoId!: string;
  apartment!: ApartmentModel;
  apiUrlImagesHttps: string = environment.apiUrlImagesHttps;
  imgPath!: string;
  authorizedUser: AuthorizedUser;
  isLoading: boolean = false;
  unexpectedError!: HttpErrorResponse | undefined;

  constructor(private router: Router, private activeRoute: ActivatedRoute, private apartmentService: ApartmentService, private localStorage: LocalStorageService) 
  {  
    this.authorizedUser = localStorage.getAuthorizedUser();
    this.activeRoute.paramMap.subscribe(x=>{
      this.apartmentId = x.get('apartmentId') as string;
      this.photoId = x.get('photoId') as string;
    })
  }

  ngOnInit(): void {
    this.changeLoadingState();
    this.wipeErrors();
    this.apartmentService.getApartmentById(this.apartmentId).subscribe({
      next: (result) =>{
        this.changeLoadingState();
        this.apartment = result.data;
        this.imgPath = this.photoId === null ? this.apartment.imageUrl : this.apartment.photos.find(x=>x.id === this.photoId)?.imageUrl as string;
      },
      error: (e: HttpErrorResponse)=>{
        this.changeLoadingState();
        this.unexpectedError = e;
      }
    });
  }
  
  closeClick(){
      this.router.navigateByUrl(`/Apartments/${this.apartmentId}`);
  }

  deletePhoto(){
    this.changeLoadingState();
    this.wipeErrors();
    if(this.photoId){
      this.apartmentService.deletePhoto(this.apartmentId, this.photoId).subscribe({
        next: () => 
        {
          this.changeLoadingState();
          this.router.navigateByUrl('/Apartments/' + this.apartmentId + '/Photos')
      }, 
        error: (e: HttpErrorResponse)=>{
        this.changeLoadingState();
        this.unexpectedError = e;
      }});
    }
    else{
      this.apartmentService.deleteMainPhoto(this.apartmentId).subscribe({
        next: () => 
        {
          this.changeLoadingState();
          this.router.navigateByUrl('/Apartments/' + this.apartmentId)
      }, 
        error: (e: HttpErrorResponse)=>{
        this.changeLoadingState();
        this.unexpectedError = e;
      }});
    }
    
  }

  wipeErrors(){
    this.unexpectedError = undefined;
  }

  changeLoadingState(){
    this.isLoading = !this.isLoading;
  }
}
