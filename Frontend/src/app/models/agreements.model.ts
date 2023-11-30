import { AgreementModel } from "./agreement-item.model";


export interface IAgreementsModel{
    agreements: AgreementModel[];

    totalAgreementCount: number;
    totalSumToPay: number;
    totalSumWasPaid: number;
}

export class AgreementsModel implements IAgreementsModel{
    public agreements: AgreementModel[];
    public totalAgreementCount!: number;
    public totalSumToPay!: number;
    public totalSumWasPaid!: number;

    constructor(agreements: AgreementModel[]){
        this.agreements = agreements;
        this.totalAgreementCount = agreements.length;
        this.totalSumToPay = 0;
        this.totalSumWasPaid = 0;
        agreements.forEach(x=>{
            this.totalSumToPay += Number(x.sumPerMonth * x.paymentsToMakeCount);
            this.totalSumWasPaid += Number(x.sumPerMonth * x.paymentsMadeCount + x.realtorPaymentSum);
        });

    }
}