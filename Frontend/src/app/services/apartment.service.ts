import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { environment } from "src/environments/environment.dev";

import { makeJWTHeader } from "../utilities/make-jwt-header";

import { ApartmentModel } from "../models/apartment.model";
import { APIResponse } from "../models/api-response";
import { LocalStorageService } from "./local-storage.service";
import { map } from "rxjs";
import { ViewModel } from "../models/view.model";

@Injectable()
export class ApartmentService{
    private apiControllerUrl: string = environment.apiUrlHttps + 'Apartments/';
    constructor(
        private localStorage: LocalStorageService,
        private httpClient: HttpClient
        ) {}

    getAllApartments(){
        return this.httpClient.get<APIResponse<ApartmentModel[]>>(this.apiControllerUrl).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    getAllApartmentsByOrderRequirements(orderId: string){
        return this.httpClient.get<APIResponse<ApartmentModel[]>>(this.apiControllerUrl + 'ByOrderRequirements/' + orderId).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    getApartmentById(id: string){
        return this.httpClient.get<APIResponse<ApartmentModel>>(this.apiControllerUrl + id).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    createApartment(model: ApartmentModel){
        model.id = '';
        model.photos = [];
        model.orders = [];
        return this.httpClient.post<APIResponse<ApartmentModel>>(this.apiControllerUrl, model).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    updateApartment(model: ApartmentModel){
        model.photos = [];
        model.orders = [];
        return this.httpClient.put<APIResponse<ApartmentModel>>(this.apiControllerUrl + model.id, model).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    deleteApartment(apartmentId: string){
        return this.httpClient.delete<APIResponse<ApartmentModel>>(this.apiControllerUrl + apartmentId).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }
    
    deleteApartmentFromAllOrders(apartmentId: string){
        return this.httpClient.delete<APIResponse<ApartmentModel>>(this.apiControllerUrl + apartmentId + '/InAllOrders').pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    addPhoto(apartmentId: string, photo: File){
        var formData = new FormData();
        formData.append('file', photo, photo.name);

        return this.httpClient.post<APIResponse<ApartmentModel>>(this.apiControllerUrl + apartmentId + '/AddPhoto', formData).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    addMainPhoto(apartmentId: string, photo: File){
        var formData = new FormData();
        formData.append('file', photo);
        
        return this.httpClient.post<APIResponse<ApartmentModel>>(this.apiControllerUrl + apartmentId + '/AddMainPhoto', formData).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    deletePhoto(apartmentId: string, photoId: string){
        return this.httpClient.put<APIResponse<ApartmentModel>>(this.apiControllerUrl + apartmentId + photoId + '/DeletePhoto',{}).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }

    deleteMainPhoto(apartmentId: string){
        return this.httpClient.put<APIResponse<ApartmentModel>>(this.apiControllerUrl + 'DeletePhoto/' + apartmentId,{}).pipe(map(response =>{
            return new ViewModel(response.data, response.errors);
        }));
    }
}