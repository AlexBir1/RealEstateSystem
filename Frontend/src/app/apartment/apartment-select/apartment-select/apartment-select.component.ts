import { HttpErrorResponse } from '@angular/common/http';
import { AfterContentInit, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApartmentModel } from 'src/app/models/apartment.model';
import { AuthorizedUser } from 'src/app/models/authorized-user.model';
import { ErrorModel } from 'src/app/models/error.model';
import { ApartmentService } from 'src/app/services/apartment.service';
import { OrderService } from 'src/app/services/order.service';
import { environment } from 'src/environments/environment.dev';

@Component({
  selector: 'app-apartment-select',
  templateUrl: './apartment-select.component.html',
  styleUrls: ['./apartment-select.component.css']
})
export class ApartmentSelectComponent implements OnInit{
  availableApartments: ApartmentModel[] = [];
  apiUrlImagesHttps: string = environment.apiUrlImagesHttps;
  isLoading: boolean = false;
  unexpectedError!: HttpErrorResponse | undefined;
  errorModalContent!: ErrorModel | undefined;
  selectedApartmentIds: string[] = [];
  orderApartments: ApartmentModel[] = [];
  orderId!: string;
  currentSelectedApartments: ApartmentModel[] = [];
  isUpdateMode: boolean = true;

  constructor(private activeRoute: ActivatedRoute, private apartmentService: ApartmentService, private orderService: OrderService, private router: Router)
  {
    this.activeRoute.paramMap.subscribe(x=> this.orderId = x.get('orderId') as string);
    
  }

  ngOnInit(): void {
    this.selectedApartmentIds = [];
    this.changeLoadingState();
    this.apartmentService.getAllApartmentsByOrderRequirements(this.orderId).subscribe({
      next: (result) =>{
      this.changeLoadingState();
      if(result.isSuccess){
        this.availableApartments = result.data;
        this.changeLoadingState();
        this.getOrderByRequestedId();
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

  getOrderByRequestedId(){
    this.orderService.getOrderById(this.orderId).subscribe({
      next: (result) =>{
      this.changeLoadingState();
      if(result.isSuccess){
        result.data.apartments.forEach(x => {
          this.availableApartments.splice(this.availableApartments.findIndex(y=>y.id == x.id),1)
          this.orderApartments.push(x);
        });
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

  changeInOrderStatus(apartmentId: string, e: any){
    var status: boolean = e.target.checked as boolean;
    if(status)
      this.selectedApartmentIds.push(apartmentId);
      else{
        this.selectedApartmentIds.splice(this.selectedApartmentIds.findIndex(x=>x == apartmentId) as number, 1)
      }
  }

  updateOrderApartments(){
    var selectedApartments: ApartmentModel[] = [];
    this.availableApartments.forEach(x=>{
      if(this.selectedApartmentIds.find(y=>y == x.id))
        selectedApartments.push(x);
    });
    this.changeLoadingState();
    this.orderService.updateOrderApartments(this.orderId, selectedApartments).subscribe({
      next: (result) =>{
      this.changeLoadingState();
      if(result.isSuccess){
        this.router.navigateByUrl('/Orders/' + this.orderId)
      }
      else{
        this.errorModalContent = new ErrorModel("Operation has failed", result.errors);
      }
    },
    error: (e: HttpErrorResponse)=>{
      this.changeLoadingState();
      this.unexpectedError = e;
    }
    })
  }

  deleteOrderApartments(){
    var selectedApartments: ApartmentModel[] = [];
    this.orderApartments.forEach(x=>{
      if(this.selectedApartmentIds.find(y=>y == x.id))
        selectedApartments.push(x);
    });
    this.changeLoadingState();
    this.orderService.deleteOrderApartments(this.orderId, selectedApartments).subscribe({
      next: (result) =>{
      this.changeLoadingState();
      if(result.isSuccess){
        this.router.navigateByUrl('/Orders/' + this.orderId)
      }
      else{
        this.errorModalContent = new ErrorModel("Operation has failed", result.errors);
      }
    },
    error: (e: HttpErrorResponse)=>{
      this.changeLoadingState();
      this.unexpectedError = e;
    }
    })
  }

  changeLoadingState(){
    this.isLoading = !this.isLoading;
  }

  changeWorkMode(){
    this.selectedApartmentIds = [];
  }

  wipeErrors(){
    this.errorModalContent = undefined;
    this.unexpectedError = undefined;
  }

  closeErrorModal(){
    this.errorModalContent = undefined;
  }
}
