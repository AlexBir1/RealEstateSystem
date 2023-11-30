import { ApartmentPhotoModel } from "./apartment-photo.model";
import { OrderModel } from "./order.model";

 
export interface ApartmentModel{
    id: string;
    number: string;
    price: number;
    rooms: number;
    imageUrl: string;

    city:string;
    address:string;

    description: string;
    realtorName: string;
    realtorPhone: string;

    orders: OrderModel[];

    isActive: boolean;
    photos: ApartmentPhotoModel[];
    creationDate: Date;
    lastlyUpdatedDate: Date;
}