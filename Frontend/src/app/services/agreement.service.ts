import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs";
import { environment } from "src/environments/environment.dev";
import { AgreementModel } from "../models/agreement-item.model";
import { AgreementsModel, IAgreementsModel } from "../models/agreements.model";
import { APIResponse } from "../models/api-response";
import { ViewModel } from "../models/view.model";
import { makeJWTHeader } from "../utilities/make-jwt-header";
import { LocalStorageService } from "./local-storage.service";

@Injectable()
export class AgreementService{
    private apiControllerUrl: string = environment.apiUrlHttps + 'Agreement/';

    constructor(
        private localStorage: LocalStorageService,
         private httpClient: HttpClient
         ) {}

    getAllAgreements(){
        var authorizedUser = this.localStorage.getAuthorizedUser();
        var headers = makeJWTHeader(authorizedUser.jwt);
        return this.httpClient.get<APIResponse<AgreementModel[]>>(this.apiControllerUrl, { headers }).pipe(map(response =>{
            return new ViewModel(new AgreementsModel(response.data), response.errors);
        }));
    }

    getAllAgreementsByAccountId(accountId: string){
        var authorizedUser = this.localStorage.getAuthorizedUser();
        var headers = makeJWTHeader(authorizedUser.jwt);
        return this.httpClient.get<APIResponse<AgreementModel[]>>(this.apiControllerUrl + 'ByAccountId/' + accountId, { headers }).pipe(map(response =>{
            return new ViewModel(new AgreementsModel(response.data), response.errors);
        }));
    }

    getAgreementById(agreementId: string){
        var authorizedUser = this.localStorage.getAuthorizedUser();
        var headers = makeJWTHeader(authorizedUser.jwt);
        return this.httpClient.get<APIResponse<AgreementModel>>(this.apiControllerUrl + agreementId, { headers }).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    createAgreement(model: AgreementModel){
        var authorizedUser = this.localStorage.getAuthorizedUser();
        var headers = makeJWTHeader(authorizedUser.jwt);
        return this.httpClient.post<APIResponse<AgreementModel>>(this.apiControllerUrl, model, { headers }).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    deleteAgreement(agreementId: string){
        var authorizedUser = this.localStorage.getAuthorizedUser();
        var headers = makeJWTHeader(authorizedUser.jwt);
        return this.httpClient.delete<APIResponse<AgreementModel>>(this.apiControllerUrl + agreementId, { headers }).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    updateAgreement(model: AgreementModel){
        var authorizedUser = this.localStorage.getAuthorizedUser();
        var headers = makeJWTHeader(authorizedUser.jwt);
        return this.httpClient.put<APIResponse<AgreementModel>>(this.apiControllerUrl + model.id, model, { headers }).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }
}