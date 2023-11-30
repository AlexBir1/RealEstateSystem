import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs";
import { environment } from "src/environments/environment.dev";
import { AccountModel } from "../models/account.model";
import { APIResponse } from "../models/api-response";
import { ChangePasswordModel } from "../models/change-password.model";
import { ViewModel } from "../models/view.model";
import { makeJWTHeader } from "../utilities/make-jwt-header";
import { LocalStorageService } from "./local-storage.service";

@Injectable()
export class AccountService{
    private apiControllerUrl: string = environment.apiUrlHttps + 'Account/';

    constructor(
         private localStorage: LocalStorageService,
         private httpClient: HttpClient
         ) {}

    getAuthorizedUser(){
        var authorizedUser = this.localStorage.getAuthorizedUser();
    }

    getAccountById(accountId: string){
        var authorizedUser = this.localStorage.getAuthorizedUser();
        var headers = makeJWTHeader(authorizedUser.jwt);
        return this.httpClient.get<APIResponse<AccountModel>>(this.apiControllerUrl + accountId, { headers }).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    updateAccount(model: AccountModel){
        var authorizedUser = this.localStorage.getAuthorizedUser();
        var headers = makeJWTHeader(authorizedUser.jwt);
        model.apartments = [];
        model.orders = [];
        return this.httpClient.put<APIResponse<AccountModel>>(this.apiControllerUrl + model.id, model, { headers }).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    changeAccountPassword(model: ChangePasswordModel){
        var authorizedUser = this.localStorage.getAuthorizedUser();
        var headers = makeJWTHeader(authorizedUser.jwt);
        return this.httpClient.put<APIResponse<AccountModel>>(this.apiControllerUrl + authorizedUser.userId + '/ChangePassword', model, { headers }).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }
}