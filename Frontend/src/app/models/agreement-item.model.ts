import { ApartmentModel } from "./apartment.model";

export interface IAgreementModel{
    id: string;
    accountId: string;
    sumPerMonth: number;
    realtorPaymentSum: number;
    apartmentNumber: string;
    apartmentCity: string;
    apartmentAddress: string;
    paymentsMadeCount: number;
    paymentsToMakeCount: number;
    monthCountBeforeExpiration: number;
    creationDate: Date;
    lastlyUpdatedDate: Date;
    isActive: boolean;
}

export class AgreementModel implements IAgreementModel{
    id: string;
    accountId: string;
    sumPerMonth: number;
    realtorPaymentSum: number;
    apartmentNumber: string;
    apartmentCity: string;
    apartmentAddress: string;
    paymentsMadeCount: number;
    paymentsToMakeCount: number;
    monthCountBeforeExpiration: number;
    creationDate: Date;
    lastlyUpdatedDate: Date;
    isActive: boolean;

    constructor(model: ApartmentModel, accountId: string, contactDurationMonths: number){
        this.id = '';
        this.accountId = accountId;

        this.sumPerMonth = model.price;
        this.realtorPaymentSum = model.price * 0.2;

        this.apartmentNumber = model.number;
        this.apartmentCity = model.city;
        this.apartmentAddress = model.address;

        this.paymentsMadeCount = 0;
        this.paymentsToMakeCount = contactDurationMonths;

        this.monthCountBeforeExpiration = contactDurationMonths;
        this.isActive = true;

        this.creationDate = new Date();
        this.lastlyUpdatedDate = new Date();
    }

}