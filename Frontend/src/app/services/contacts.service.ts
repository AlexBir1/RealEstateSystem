import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs";
import { environment } from "src/environments/environment.dev";
import { APIResponse } from "../models/api-response";
import { ContactModel } from "../models/contact.model";
import { ViewModel } from "../models/view.model";
import { makeJWTHeader } from "../utilities/make-jwt-header";
import { LocalStorageService } from "./local-storage.service";

@Injectable()
export class ContactsService{
    private apiControllerUrl: string = environment.apiUrlHttps + 'Contacts/';

    constructor(
        private localStorage: LocalStorageService,
         private httpClient: HttpClient
         ) {}

    getContacts(){
        return this.httpClient.get<APIResponse<ContactModel[]>>(this.apiControllerUrl).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    createContact(model: ContactModel){
        var authorizedUser = this.localStorage.getAuthorizedUser();
        var headers = makeJWTHeader(authorizedUser.jwt);
        return this.httpClient.post<APIResponse<ContactModel>>(this.apiControllerUrl, model, { headers }).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    deleteContact(contactId: string){
        var authorizedUser = this.localStorage.getAuthorizedUser();
        var headers = makeJWTHeader(authorizedUser.jwt);
        return this.httpClient.delete<APIResponse<ContactModel>>(this.apiControllerUrl + contactId, { headers }).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }
}