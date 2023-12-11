import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, ReplaySubject, Subject, tap } from "rxjs";
import { environment } from "src/environments/environment.dev";
import { APIResponse } from "../models/api-response";
import { AuthorizedUser } from "../models/authorized-user.model";
import { LogInModel } from "../models/log-in.model";
import { SignUpModel } from "../models/sign-up.model";
import { ViewModel } from "../models/view.model";
import { makeJWTHeader } from "../utilities/make-jwt-header";
import { LocalStorageService } from "./local-storage.service";

@Injectable({providedIn: 'root'})
export class AuthService{
    private apiControllerUrl: string = environment.apiUrlHttps + 'Auth/';
    private authorizedUser: ReplaySubject<AuthorizedUser> = new ReplaySubject<AuthorizedUser>(1);
    public authorizedUser$ = this.authorizedUser.asObservable();

    constructor(
         private httpClient: HttpClient,
         private localStorage: LocalStorageService
         ) {}
         
    updateAuthorizedUser(model: AuthorizedUser){
        this.authorizedUser.next(model);
        this.localStorage.setAuthorizedUser(model);
    }

    refreshAuthToken(model: AuthorizedUser){
        return this.httpClient.put<APIResponse<AuthorizedUser>>(this.apiControllerUrl + 'RefreshAuthToken', model).pipe(
            map(response =>{
                return new ViewModel(response.data, response.errors);
            })
        );
    }

    signUp(model: SignUpModel){
        return this.httpClient.post<APIResponse<AuthorizedUser>>(this.apiControllerUrl + 'SignUp', model).pipe(
            tap(response => {
                var responseData = response.data;
                this.authorizedUser.next(responseData);
            }),
            map(response =>{
                return new ViewModel(response.data, response.errors);
        })
        );
    }

    signIn(model: LogInModel){
        return this.httpClient.post<APIResponse<AuthorizedUser>>(this.apiControllerUrl + 'LogIn', model).pipe(
            tap(response => {
                var responseData = response.data;
                this.authorizedUser.next(responseData);
            }),
            map(response =>{
                return new ViewModel(response.data, response.errors);
        }));
    }

    signOut(){
        var authorizedUser = this.localStorage.getAuthorizedUser();
        var headers = makeJWTHeader(authorizedUser.jwt);
        return this.httpClient.get<APIResponse<AuthorizedUser>>(this.apiControllerUrl + 'LogOut', { headers }).pipe(
            tap(response => {
                var responseData = response.data;
                this.authorizedUser.next(responseData);
                this.localStorage.setAuthorizedUser(responseData);
            }),
            map(response =>{
                return new ViewModel(response.data, response.errors);
        }));
    }
}