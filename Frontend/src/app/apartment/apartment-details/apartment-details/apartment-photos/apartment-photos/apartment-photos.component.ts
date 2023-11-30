import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApartmentPhotoModel } from 'src/app/models/apartment-photo.model';
import { ApartmentModel } from 'src/app/models/apartment.model';
import { ApartmentService } from 'src/app/services/apartment.service';
import { environment } from 'src/environments/environment.dev';

@Component({
  selector: 'app-apartment-photos',
  templateUrl: './apartment-photos.component.html',
  styleUrls: ['./apartment-photos.component.css']
})
export class ApartmentPhotosComponent implements OnInit
{
  apartmentId!: string;
  mainPhotoPath!: string;
  photos: ApartmentPhotoModel[] = [];
  apiUrlImagesHttps: string = environment.apiUrlImagesHttps;
  isLoading: boolean = false;
  unexpectedError!: HttpErrorResponse | undefined;

  constructor(private router: Router, private activeRoute: ActivatedRoute, private apartmentService: ApartmentService) 
  {  
    this.activeRoute.paramMap.subscribe(x=>{
      this.apartmentId = x.get('apartmentId') as string;
    })
  }
  ngOnInit(): void {
    this.getPhotos();
  }
  
  wipeErrors(){
    this.unexpectedError = undefined;
  }

  closeClick(){
    this.activeRoute.paramMap.subscribe(x=>{
      this.router.navigateByUrl(`/Apartments/${this.apartmentId}`);
    })
  }

  getPhotos(){
    this.changeLoadingState();
    this.wipeErrors();
    this.apartmentService.getApartmentById(this.apartmentId).subscribe({next: (result) => 
      {
        this.changeLoadingState();
        this.photos = result.data.photos; 
        this.mainPhotoPath = result.data.imageUrl;
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
}
