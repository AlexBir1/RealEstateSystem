import { OrderStatus } from "../enums/order-status";
import { ApartmentModel } from "./apartment.model";

export interface OrderModel{
    id: string;
    accountId: string;
    creationDate: Date;
    lastlyUpdatedDate: Date;
    orderStatus: OrderStatus;

    city: string;
    estimatedRoomsQuantity: number;
    estimatedPriceLimit: number;

    apartments: ApartmentModel[];
}

