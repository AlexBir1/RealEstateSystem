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
        var authorizedUser = this.localStorage.getAuthorizedUser();
        var headers = makeJWTHeader(authorizedUser.jwt);
        return this.httpClient.get<APIResponse<OrderModel[]>>(this.apiControllerUrl, { headers }).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    getAllOrdersByAccountId(accountId: string){
        var authorizedUser = this.localStorage.getAuthorizedUser();
        var headers = makeJWTHeader(authorizedUser.jwt);
        return this.httpClient.get<APIResponse<OrderModel[]>>(this.apiControllerUrl + 'ByAccountId/' + accountId, { headers }).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    getOrderById(orderId: string){
        var authorizedUser = this.localStorage.getAuthorizedUser();
        var headers = makeJWTHeader(authorizedUser.jwt);
        return this.httpClient.get<APIResponse<OrderModel>>(this.apiControllerUrl + orderId, { headers }).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    deleteOrder(orderId: string){
        var authorizedUser = this.localStorage.getAuthorizedUser();
        var headers = makeJWTHeader(authorizedUser.jwt);
        return this.httpClient.delete<APIResponse<OrderModel>>(this.apiControllerUrl + orderId, { headers }).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    createOrder(model: OrderModel){
        var authorizedUser = this.localStorage.getAuthorizedUser();
        var headers = makeJWTHeader(authorizedUser.jwt);
        return this.httpClient.post<APIResponse<OrderModel>>(this.apiControllerUrl, model, { headers }).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    updateOrder(model: OrderModel){
        var authorizedUser = this.localStorage.getAuthorizedUser();
        var headers = makeJWTHeader(authorizedUser.jwt);
        return this.httpClient.put<APIResponse<OrderModel>>(this.apiControllerUrl + model.id, model, { headers }).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }
}