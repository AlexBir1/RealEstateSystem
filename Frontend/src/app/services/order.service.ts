import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs";
import { environment } from "src/environments/environment.dev";
import { APIResponse } from "../models/api-response";
import { OrderModel } from "../models/order.model";
import { ViewModel } from "../models/view.model";
import { makeJWTHeader } from "../utilities/make-jwt-header";
import { LocalStorageService } from "./local-storage.service";

@Injectable()
export class OrderService{
    private apiControllerUrl: string = environment.apiUrlHttps + 'Order/';

    constructor(
        private localStorage: LocalStorageService,
        private httpClient: HttpClient
        ) {}

    getAllOrders(){
        return this.httpClient.get<APIResponse<OrderModel[]>>(this.apiControllerUrl).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    getAllOrdersByAccountId(accountId: string){
        return this.httpClient.get<APIResponse<OrderModel[]>>(this.apiControllerUrl + 'ByAccountId/' + accountId).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    getOrderById(orderId: string){
        return this.httpClient.get<APIResponse<OrderModel>>(this.apiControllerUrl + orderId).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    deleteOrder(orderId: string){
        return this.httpClient.delete<APIResponse<OrderModel>>(this.apiControllerUrl + orderId).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    createOrder(model: OrderModel){
        return this.httpClient.post<APIResponse<OrderModel>>(this.apiControllerUrl, model).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    updateOrder(model: OrderModel){
        return this.httpClient.put<APIResponse<OrderModel>>(this.apiControllerUrl + model.id, model).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }
}