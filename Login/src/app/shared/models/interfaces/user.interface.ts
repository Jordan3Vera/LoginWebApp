export interface IUser{
    id?: any;
    firstname?: string;
    lastname?: string;
    email?: string;
    password?: string;
    confirmPassword?: string;
}

export interface ILogin{
    email: string;
    password: string;
    accessToken?: any;
    user?: {
        email: string,
        firstname: string,
        lastname: string
    }
}

export class IUserRest{
    userId?: any;
    userName?: string;
    email?: string;
    password?: string;
    confirmPassword?: string;
}