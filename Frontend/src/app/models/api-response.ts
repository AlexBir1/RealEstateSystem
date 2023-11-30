export interface APIResponse<T>{
    data: T;
    errors: string[];
}