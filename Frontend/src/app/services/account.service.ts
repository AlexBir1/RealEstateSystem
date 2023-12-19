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
        return this.localStorage.getAuthorizedUser();
    }

    getAccountById(accountId: string){
        return this.httpClient.get<APIResponse<AccountModel>>(this.apiControllerUrl + accountId).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    updateAccount(model: AccountModel){
        return this.httpClient.put<APIResponse<AccountModel>>(this.apiControllerUrl + model.id, model).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    changeAccountPassword(model: ChangePasswordModel){
        return this.httpClient.put<APIResponse<AccountModel>>(this.apiControllerUrl + this.getAuthorizedUser().userId + '/ChangePassword', model).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }
}