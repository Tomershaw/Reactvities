// Represents a user in the application
export interface User {
  username: string; // The unique username of the user
  displayName: string; // The display name of the user
  token: string; // Authentication token for the user
  image?: string; // Optional profile image URL
  userFormValues?: UserFormValues; // Optional form values associated with the user
  canCreateActivity: boolean; // Indicates if the user can create activities
}

// Represents form values for user-related actions
export interface UserFormValues {
  email: string; // The user's email address
  password: string; // The user's password
  displayName?: string; // Optional display name for the user
  username?: string; // Optional username for the user
}
