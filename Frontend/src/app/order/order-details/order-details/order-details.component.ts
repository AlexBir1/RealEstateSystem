import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthorizedUser } from 'src/app/models/authorized-user.model';
import { ErrorModel } from 'src/app/models/error.model';
import { OrderModel } from 'src/app/models/order.model';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.css']
})
export class OrderDetailsComponent implements OnInit{
  orderId!: string;
  order!: OrderModel;
  authorizedUser: AuthorizedUser;
  isLoading: boolean = false;
  unexpectedError!: HttpErrorResponse | undefined;
  errorModalContent!: ErrorModel | undefined;

  constructor(private activeRoute: ActivatedRoute, private orderService: OrderService, private localStorage: LocalStorageService, private router: Router){
    this.authorizedUser = localStorage.getAuthorizedUser();
    this.activeRoute.paramMap.subscribe(x=> this.orderId = x.get('orderId') as string);
  }
  ngOnInit(): void {
    this.getOrder();
  }


  getOrder(){
    this.changeLoadingState();
    this.orderService.getOrderById(this.orderId).subscribe({
      next: (result) => {
        this.changeLoadingState();
        if(result.isSuccess){
          this.order = result.data;
        }
        else{
          this.errorModalContent = new ErrorModel("Operation has failed", result.errors);
        }
      },
      error: (e) => {
        this.changeLoadingState();
        this.unexpectedError = e;
      }
    });
  }

  deleteOrder(){
    this.orderService.deleteOrder(this.order.id).subscribe({next: (result) => {
      if(result.isSuccess){
        this.changeLoadingState();
        this.router.navigateByUrl('/Orders');
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

  addApartments(){
    this.router.navigateByUrl('/Apartments/ByOrderRequirements/' + this.order.id);
  }

  changeLoadingState(){
    this.isLoading = !this.isLoading;
  }

  wipeErrors(){
    this.errorModalContent = undefined;
    this.unexpectedError = undefined;
  }

  closeErrorModal(){
    this.errorModalContent = undefined;
  }
}
