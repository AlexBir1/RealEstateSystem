export interface ChangePasswordModel{
    accountId: string;
    oldPassword: string;
    newPassword: string;
    newPasswordConfirm: string;
}