import { User } from "./user";

export interface IProfile{
    userName: string;
    displayName:string;
    image?:string;
    bio?:string;
    followersCount:number;
    followingCount:number;
    following:boolean;
    photos?:Photo[];    
}

export interface  UserActivity{
    id:number
    Title:string
    category:string
    Date:Date
    // HostUsername :string 
}

export class Profile implements IProfile{
    constructor(user:User){
        this.userName = user.username;
        this.displayName =user.displayName;
        this.image =user.image
    }

    userName: string;
    displayName:string;
    image?:string;
    bio?:string;
    followersCount=0;
    followingCount=0;
    following= false;
    photos?:Photo[]; 
}

export default interface Photo
{
    id:string;
    url:string;
    isMain:boolean;
}