import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs";
import { environment } from "src/environments/environment.dev";
import { APIResponse } from "../models/api-response";
import { CallModel } from "../models/call.model";
import { RequestCallModel } from "../models/request-call.model";
import { ViewModel } from "../models/view.model";
import { makeJWTHeader } from "../utilities/make-jwt-header";
import { LocalStorageService } from "./local-storage.service";

@Injectable()
export class CallService{
    private apiControllerUrl: string = environment.apiUrlHttps + 'Call/';

    constructor(
        private localStorage: LocalStorageService,
         private httpClient: HttpClient
         ) {}

    getCalls(){
        var authorizedUser = this.localStorage.getAuthorizedUser();
        var headers = makeJWTHeader(authorizedUser.jwt);
        return this.httpClient.get<APIResponse<CallModel[]>>(this.apiControllerUrl, { headers }).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    requestCall(model: RequestCallModel){
        var authorizedUser = this.localStorage.getAuthorizedUser();
        var headers = makeJWTHeader(authorizedUser.jwt);
        return this.httpClient.post<APIResponse<CallModel>>(this.apiControllerUrl, model, { headers }).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    deleteCallRequest(callId: string){
        var authorizedUser = this.localStorage.getAuthorizedUser();
        var headers = makeJWTHeader(authorizedUser.jwt);
        return this.httpClient.delete<APIResponse<CallModel>>(this.apiControllerUrl + callId, { headers }).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }
}