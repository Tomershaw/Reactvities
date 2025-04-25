import { makeAutoObservable, reaction } from "mobx";
import { ServerError } from "../models/serverError";

// MobX store for managing common application state
export default class CommonStore {
  // Stores server error details
  error: ServerError | null = null;
  // JWT token for authentication, persisted in localStorage
  token: string | null = localStorage.getItem("jwt");
  // Tracks if the application has fully loaded
  appLoaded = false;

  constructor() {
    makeAutoObservable(this);
    // Reacts to token changes and updates localStorage accordingly
    reaction(
      () => this.token,
      token => {
        if (token) {
          localStorage.setItem("jwt", token);
        } else {
          localStorage.removeItem("jwt");
        }
      }
    );
  }

  // Sets the server error state
  setServerError(error: ServerError) {
    this.error = error;
  }

  // Updates the JWT token state
  setToken = (token: string | null) => {
    this.token = token;
  };

  // Marks the application as fully loaded
  setAppLoaded = () => {
    this.appLoaded = true;
  };
}
