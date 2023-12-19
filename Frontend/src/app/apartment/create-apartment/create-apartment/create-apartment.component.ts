import { HttpErrorResponse } from '@angular/common/http';
import { Component, EventEmitter, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApartmentPhotoModel } from 'src/app/models/apartment-photo.model';
import { ApartmentModel } from 'src/app/models/apartment.model';
import { ErrorModel } from 'src/app/models/error.model';
import { ApartmentService } from 'src/app/services/apartment.service';
import { environment } from 'src/environments/environment.dev';

@Component({
  selector: 'app-create-apartment',
  templateUrl: './create-apartment.component.html',
  styleUrls: ['./create-apartment.component.css']
})
export class CreateApartmentComponent implements OnInit {

  apartment!: ApartmentModel;
  createApartmentForm!: FormGroup;
  uploadedImages: ApartmentPhotoModel[] = [];
  uploadedMainImagePath: string = '';
  isEdit: boolean = false;
  apiUrlImagesHttps: string = environment.apiUrlImagesHttps;
  isLoading: boolean = false;
  errorModalContent!: ErrorModel | undefined;
  unexpectedError!: HttpErrorResponse | undefined;
  confirmationRequested: boolean = false;
  action!: Function;

  constructor(private activeRoute: ActivatedRoute, private apartmentService: ApartmentService, private router: Router){}

  ngOnInit(): void {
    this.setupApartmentForm();
    var id = this.activeRoute.snapshot.paramMap.get('apartmentId')
    if(id){
      this.getApartment(id);
      this.isEdit = true;
    }
  }

  changeLoadingState(){
    this.isLoading = !this.isLoading;
  }

  setupApartmentForm(){
    this.createApartmentForm = new FormGroup({
      number: new FormControl('', Validators.required),
      price: new FormControl('', Validators.required),
      rooms: new FormControl('', [Validators.required]),
      realtorName: new FormControl('', Validators.required),
      realtorPhone: new FormControl('', Validators.required),
      description: new FormControl('', Validators.required),
      city: new FormControl('', [Validators.required]),
      address: new FormControl('', Validators.required),
      imageUrl: new FormControl(''),
    });
  }

  deleteApartment(){
    this.changeLoadingState();
    this.apartmentService.deleteApartment(this.apartment.id).subscribe({
      next: (result) => {
        this.changeLoadingState();
      if(result.isSuccess){
        this.router.navigateByUrl('/Apartments');
      }
      else{
        this.errorModalContent = new ErrorModel("Operation has failed", result.errors);
      }
      },
      error: (e: HttpErrorResponse)=>{
        this.unexpectedError = e;
      }
    })
  }

  getApartment(apartmentId: string){
    this.apartmentService.getApartmentById(apartmentId).subscribe(result => {
      this.apartment = result.data;
      this.uploadedImages = result.data.photos;

      this.createApartmentForm.controls['number'].setValue(this.apartment.number);
      this.createApartmentForm.controls['price'].setValue(this.apartment.price);
      this.createApartmentForm.controls['rooms'].setValue(this.apartment.rooms);
      this.createApartmentForm.controls['realtorName'].setValue(this.apartment.realtorName);
      this.createApartmentForm.controls['realtorPhone'].setValue(this.apartment.realtorPhone);
      this.createApartmentForm.controls['description'].setValue(this.apartment.description);
      this.createApartmentForm.controls['city'].setValue(this.apartment.city);
      this.createApartmentForm.controls['address'].setValue(this.apartment.address);
      this.createApartmentForm.controls['imageUrl'].setValue(this.apartment.imageUrl);
    });
  }

  onMainPhotoInputChange(e: any){
    var file = e?.target?.files[0] as File;
    this.changeLoadingState();
    this.apartmentService.addMainPhoto(this.apartment.id, file).subscribe({
      next: (r)=>{
        this.changeLoadingState();
        if(r.isSuccess){
          this.apartment.imageUrl = r.data.imageUrl;
          this.createApartmentForm.controls['imageUrl'].setValue(r.data.imageUrl);
          e!.target!.files = [];
        }
      },
      error: (e)=>{
        this.changeLoadingState();
        this.unexpectedError = e;
      },
    });
  }

  onPhotoInputChange(e: any){
    var file = e?.target?.files[0] as File;
    this.changeLoadingState();
    this.apartmentService.addPhoto(this.apartment.id, file).subscribe({

      next: (r)=>{
        this.changeLoadingState();
        if(r.isSuccess){
          this.apartment.photos.push(r.data);
          e!.target!.files = [];
        }
      },
      error: (e)=>{
        this.changeLoadingState();
        this.unexpectedError = e;
      },
    });
  }

  onSubmit(){
    this.changeLoadingState();
    if(!this.isEdit){
    this.apartmentService.createApartment(this.createApartmentForm.value).subscribe({
      next: (result) =>{
      this.changeLoadingState();
      if(result.isSuccess){
        this.apartment = result.data;
        this.isEdit = true;
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
    else{
      var apartment: ApartmentModel = this.createApartmentForm.value;
      apartment.id = this.apartment.id;
      this.apartmentService.updateApartment(apartment).subscribe({
      next: (result) =>{
      this.changeLoadingState();
      if(result.isSuccess){
        var currentPhotos = this.apartment.photos;
        this.apartment = result.data;
        this.apartment.photos = currentPhotos;
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

  closeErrorModal(){
    this.errorModalContent = undefined;
  }

  requestOrDeclineConfirmation(func: Function){
    this.action = func;
    this.confirmationRequested = !this.confirmationRequested;
  }

  deletePhoto(photoId: string = ''){
    this.changeLoadingState();
    if(photoId){
      this.apartmentService.deletePhoto(this.apartment.id, photoId).subscribe({
        next: (r) => 
        {
          this.changeLoadingState();
          if(r.isSuccess){
            var index = this.apartment.photos.findIndex(x=>x.id == r.data.id);
            this.apartment.photos.splice(index,1);
          }
          else{
            this.errorModalContent = new ErrorModel("Operation has failed", r.errors);
          }
      }, 
        error: (e: HttpErrorResponse)=>{
        this.changeLoadingState();
        this.unexpectedError = e;
      }});
    }
    else{
      this.apartmentService.deleteMainPhoto(this.apartment.id).subscribe({
        next: (r) => 
        {
          this.changeLoadingState();
          if(r.isSuccess){
            this.apartment.imageUrl = '';
            this.createApartmentForm.controls['imageUrl'].setValue('');
          }
          else{
            this.errorModalContent = new ErrorModel("Operation has failed", r.errors);
          }
        }, 
        error: (e: HttpErrorResponse)=>{
        this.changeLoadingState();
        this.unexpectedError = e;
      }});
    }
    
  }
}
