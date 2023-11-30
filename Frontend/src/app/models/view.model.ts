export interface IViewModel<T>{
    isSuccess: boolean;
    data: T;
    errors: string[];
}

export class ViewModel<T> implements IViewModel<T>{
    public isSuccess!: boolean;
    public data!: T;
    public errors!: string[];
    
    constructor(data: T, errors: string[]){
        this.data = data;
        this.isSuccess = (data ? true : false);
        this.errors = errors;
    }
}