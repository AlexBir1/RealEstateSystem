export interface AuthorizedUser{
    userId: string;
    jwt: string;
    role: string;
    keepAuthorized: boolean;
    tokenExpirationDate: Date;
}