import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
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

  constructor(private activeRoute: ActivatedRoute, private apartmentService: ApartmentService){}

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
    });
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
    });
  }

  onMainPhotoInputChange(e: any){
    var file = e?.target?.files[0] as File;
    this.apartmentService.addMainPhoto(this.apartment.id, file).subscribe(result=>this.apartment.imageUrl = result.data.imageUrl);
  }

  onPhotoInputChange(e: any){
    var file = e?.target?.files[0] as File;
    this.apartmentService.addPhoto(this.apartment.id, file).subscribe(result=>this.apartment.photos = result?.data?.photos as ApartmentPhotoModel[]);
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
        this.apartment = result.data;
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
}
