import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AuthorizedUser } from '../models/authorized-user.model';
import { ErrorModel } from '../models/error.model';
import { OrderModel } from '../models/order.model';
import { LocalStorageService } from '../services/local-storage.service';
import { OrderService } from '../services/order.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit{
  orders: OrderModel[] = [];
  authorizedUser: AuthorizedUser;
  isLoading: boolean = false;
  unexpectedError!: HttpErrorResponse | undefined;
  errorModalContent!: ErrorModel | undefined;
  
  constructor(private orderService: OrderService, private localStorage: LocalStorageService){
    this.authorizedUser = localStorage.getAuthorizedUser();
  }
  
  ngOnInit(): void {
    if(this.authorizedUser.role === 'Admin')
      this.getAllOrders();
    else
      this.getOrdersByAccountId();
  }

  getOrdersByAccountId(){
    this.changeLoadingState();
    this.wipeErrors();
    this.orderService.getAllOrdersByAccountId(this.authorizedUser.userId).subscribe({
      next: (result) => 
      {
        this.changeLoadingState();
        if(result.isSuccess){
          this.orders = result.data;
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

  getAllOrders(){
    this.changeLoadingState();
    this.wipeErrors();
    this.orderService.getAllOrders().subscribe({
      next: (result) => 
      {
        this.changeLoadingState();
        if(result.isSuccess){
          this.orders = result.data;
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
