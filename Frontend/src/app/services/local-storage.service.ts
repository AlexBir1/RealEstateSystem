import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment.dev";
import { AuthorizedUser } from "../models/authorized-user.model";

@Injectable()
export class LocalStorageService{
    setAuthorizedUser(model: AuthorizedUser){
        localStorage.setItem(environment.authorizedUserKey, JSON.stringify(model));
        return model;
    }
    getAuthorizedUser(){
        var userString = localStorage.getItem(environment.authorizedUserKey) as string;
        var authorizedUser: AuthorizedUser = JSON.parse(userString);
        return authorizedUser;
    }
}