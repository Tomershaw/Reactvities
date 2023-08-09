import { User } from "./user";

export interface Profile{
    userName: string;
    displayName:string;
    image?:string;
    bio?:string;
    photos?:photo[];    
}

export class Profile implements Profile{
    constructor(user:User){
        this.userName = user.username;
        this.displayName =user.displayName;
        this.image =user.image
    }
}

export default interface photo
{
    id:string;
    url:string;
    isMain:boolean;
}