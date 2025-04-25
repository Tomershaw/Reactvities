// MobX store for managing user authentication and related actions
import { makeAutoObservable, runInAction } from "mobx";
import { User, UserFormValues } from "../models/user"; // User model and form values
import agent from "../api/agent"; // API agent for server communication
import { store } from "./store"; // Root store for shared state
import { router } from "../router/Routes"; // Router for navigation

export default class UserStore {
  user: User | null = null; // Current logged-in user
  userFormValues: UserFormValues | null = null; // Form values for user login/register
  fbLoading = false; // Loading state for Facebook login
  refreshTokenTimeout: any; // Timer for refreshing tokens

  constructor() {
    makeAutoObservable(this); // Makes the store observable for MobX
  }

  // Returns the email of the current user if available
  get HostUsers() {
    if (store.userStore.user) {
      return store.userStore.user.userFormValues?.email;
    }
    return false;
  }

  // Checks if a user is logged in
  get isLoggedIn() {
    return !!this.user;
  }

  // Logs in a user with provided credentials
  login = async (creds: UserFormValues) => {
    try {
      const user = await agent.Account.login(creds);
      store.commonStore.setToken(user.token); // Stores the JWT token
      this.startRefreshTokenTimer(user); // Starts token refresh timer
      runInAction(() => {
        this.user = user;
      });
      router.navigate("/activities"); // Navigates to activities page
      store.modalStore.closeModal(); // Closes any open modal
    } catch (error) {
      console.log(error);
      throw error;
    }
  };

  // Registers a new user
  regsiter = async (creds: UserFormValues) => {
    try {
      const user = await agent.Account.regsiter(creds);
      store.commonStore.setToken(user.token);
      this.startRefreshTokenTimer(user);
      runInAction(() => (this.user = user));
      router.navigate("/activities");
      store.modalStore.closeModal();
    } catch (error) {
      console.log(error);
      throw error;
    }
  };

  // Logs out the current user
  logout = () => {
    store.commonStore.setToken(null);
    this.user = null;
    router.navigate("/");
  };

  // Fetches the current user from the server
  getUser = async () => {
    try {
      const user = await agent.Account.current();
      store.commonStore.setToken(user.token);
      this.startRefreshTokenTimer(user);
      runInAction(() => (this.user = user));
    } catch (error) {
      console.log(error);
    }
  };

  // Updates the user's profile image
  setImage = (image: string) => {
    if (this.user) this.user.image = image;
  };

  // Logs in a user via Facebook
  facebookLogin = async (accessToken: string) => {
    try {
      this.fbLoading = true;
      const user = await agent.Account.fbLogin(accessToken);
      store.commonStore.setToken(user.token);
      this.startRefreshTokenTimer(user);
      runInAction(() => {
        this.user = user;
        this.fbLoading = false;
      });
      router.navigate("/activities");
    } catch (error) {
      console.log(error);
      runInAction(() => (this.fbLoading = false));
    }
  };

  // Refreshes the user's token
  refreshToken = async () => {
    this.stopRefreshTokenTimer();
    try {
      const user = await agent.Account.refreshToken();
      runInAction(() => (this.user = user));
      store.commonStore.setToken(user.token);
      this.startRefreshTokenTimer(user);
    } catch (error) {
      console.log(error);
    }
  };

  // Starts a timer to refresh the token before it expires
  private startRefreshTokenTimer(user: User) {
    const jwtToken = JSON.parse(atob(user.token.split(".")[1]));
    const expires = new Date(jwtToken.exp * 1000);
    const timeout = expires.getTime() - Date.now() - 60 * 1000;
    this.refreshTokenTimeout = setTimeout(this.refreshToken, timeout);
  }

  // Stops the token refresh timer
  private stopRefreshTokenTimer() {
    clearTimeout(this.refreshTokenTimeout);
  }
}
