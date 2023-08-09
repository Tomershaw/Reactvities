export interface User{
    username:string ;
    displayName:string ;
    token:string;
    image?: string ;
    userFormValues?:UserFormValues;
    canCreateActivity:boolean;
    
}

export interface UserFormValues{
    email:string ;
    password:string ;
    displayName?: string ;
    username?:string;
}