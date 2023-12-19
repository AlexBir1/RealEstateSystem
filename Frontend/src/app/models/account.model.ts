import { ApartmentModel } from "./apartment.model";
import { OrderModel } from "./order.model";

export interface AccountModel{
    id: string;
    fullname: string;
    username: string;
    email: string;
    mobilePhone: string;
    role: string;
    creationDate: Date;
    lastlyUpdatedDate: Date;
}