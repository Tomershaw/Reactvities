import { Profile } from "./profile";

// Represents the structure of an activity object
export interface IActivity {
  id: string; // Unique identifier for the activity
  title: string; // Title of the activity
  date: Date | null; // Date of the activity, nullable
  description: string; // Description of the activity
  category: string; // Category of the activity
  city: string; // City where the activity takes place
  venue: string; // Venue of the activity
  hostUsername?: string; // Optional: Username of the host
  isCancelled?: boolean; // Optional: Indicates if the activity is cancelled
  isGoing?: boolean; // Optional: Indicates if the user is attending
  isHost?: boolean; // Optional: Indicates if the user is the host
  host?: Profile; // Optional: Host profile information
  attendees: Profile[]; // List of attendees' profiles
}

// Class representing an activity with default values and initialization logic
export class Activity implements IActivity {
  constructor(init: ActivityFormValues) {
    // Initialize properties from the provided form values
    this.id = init.id!;
    this.title = init.title;
    this.date = init.date;
    this.description = init.description;
    this.category = init.category;
    this.venue = init.venue;
    this.city = init.city;
  }

  id: string;
  title: string;
  date: Date | null;
  description: string;
  category: string;
  city: string;
  venue: string;
  hostUsername?: string;
  isCancelled?: boolean;
  isGoing?: boolean;
  isHost?: boolean;
  host?: Profile;
  attendees: Profile[] = []; // Default to an empty list of attendees
}

// Class for managing activity form values with default initialization
export class ActivityFormValues {
  id?: string = undefined; // Optional: Unique identifier for the activity
  title: string = ""; // Default title
  category: string = ""; // Default category
  description: string = ""; // Default description
  date: Date | null = null; // Default date, nullable
  city: string = ""; // Default city
  venue: string = ""; // Default venue

  constructor(activity?: ActivityFormValues) {
    if (activity) {
      // Copy values from the provided activity object
      this.id = activity.id;
      this.title = activity.title;
      this.category = activity.category;
      this.description = activity.description;
      this.date = activity.date;
      this.venue = activity.venue;
      this.city = activity.city;
    }
  }
}
