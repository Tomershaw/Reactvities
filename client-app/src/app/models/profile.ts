import { User } from "./user";

// Represents a user profile with various details such as username, display name, image, bio, and follower information.
export interface IProfile {
  userName: string; // The unique username of the profile.
  displayName: string; // The display name of the user.
  image?: string; // Optional profile image URL.
  bio?: string; // Optional biography of the user.
  followersCount: number; // Number of followers the user has.
  followingCount: number; // Number of users this profile is following.
  following: boolean; // Indicates if the current user is following this profile.
  photos?: Photo[]; // Optional list of photos associated with the profile.
}

// Represents an activity associated with a user.
export interface UserActivity {
  id: number; // Unique identifier for the activity.
  Title: string; // Title of the activity.
  category: string; // Category of the activity.
  Date: Date; // Date of the activity.
  // HostUsername :string
}

// Class implementation of a user profile, initialized with a User object.
export class Profile implements IProfile {
  constructor(user: User) {
    this.userName = user.username; // Maps the username from the User object.
    this.displayName = user.displayName; // Maps the display name from the User object.
    this.image = user.image; // Maps the profile image from the User object.
  }

  userName: string; // The unique username of the profile.
  displayName: string; // The display name of the user.
  image?: string; // Optional profile image URL.
  bio?: string; // Optional biography of the user.
  followersCount = 0; // Default number of followers is 0.
  followingCount = 0; // Default number of following users is 0.
  following = false; // Default following status is false.
  photos?: Photo[]; // Optional list of photos associated with the profile.
}

// Represents a photo associated with a user profile.
export default interface Photo {
  id: string; // Unique identifier for the photo.
  url: string; // URL of the photo.
  isMain: boolean; // Indicates if this photo is the main profile photo.
}
