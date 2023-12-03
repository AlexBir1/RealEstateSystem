import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { LocalStorageService } from "../services/local-storage.service";
import { makeJWTHeader } from "../utilities/make-jwt-header";

@Injectable()
export class AuthInterceptor implements HttpInterceptor{
    constructor(private localStorage: LocalStorageService){}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        var authorizedUser = this.localStorage.getAuthorizedUser();

        if(authorizedUser.userId === '')
            return next.handle(req);
        
        var modifiedRequest: HttpRequest<any> = req.clone({headers: makeJWTHeader(authorizedUser.jwt)});
        return next.handle(modifiedRequest);
    }

}