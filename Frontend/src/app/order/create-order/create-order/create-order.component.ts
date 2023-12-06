import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthorizedUser } from 'src/app/models/authorized-user.model';
import { ErrorModel } from 'src/app/models/error.model';
import { OrderModel } from 'src/app/models/order.model';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-create-order',
  templateUrl: './create-order.component.html',
  styleUrls: ['./create-order.component.css']
})
export class CreateOrderComponent {
  order!: OrderModel;
  orderForm!: FormGroup;
  authorizedUser: AuthorizedUser;
  errorModalContent!: ErrorModel | undefined;
  isLoading: boolean = false;

  constructor(private localStorage: LocalStorageService, private orderService: OrderService, private router: Router){
    this.setupLogInForm();
    this.authorizedUser = localStorage.getAuthorizedUser();
  }

  setupLogInForm(){
    this.orderForm = new FormGroup({
      city: new FormControl('', Validators.required),
      estimatedPriceLimit: new FormControl('', Validators.required),
      estimatedRoomsQuantity: new FormControl('', Validators.required),
    });
  }

  createOrder(){
    var newOrder: OrderModel = this.orderForm.value;
    newOrder.accountId = this.authorizedUser.userId;
    newOrder.id = '';
    newOrder.apartments = [];
    this.isLoading = true;
    this.orderService.createOrder(newOrder).subscribe(result=>{
      this.isLoading = false;
      if(result.isSuccess)
        this.router.navigateByUrl('/Orders');
        else
          this.errorModalContent = new ErrorModel("Operation has failed", result.errors);
    });
  }

  changeLoadingState(){
    this.isLoading = !this.isLoading;
  }

  closeErrorModal(){
    this.errorModalContent = undefined;
  }
}
