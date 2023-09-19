import { makeAutoObservable, runInAction } from "mobx";
import { User, UserFormValues } from "../models/user";
import agent from "../api/agent";
import { store } from "./store";
import { router } from "../router/Routes";

export default class UserStore {
  user: User | null = null;
  userFormValues: UserFormValues | null = null;
  fbLoading = false;
  refreshTokenTimeout: any

  constructor() {
    makeAutoObservable(this);
  }

  get HostUsers() {
    if (store.userStore.user) {
      return store.userStore.user.userFormValues?.email;
    }
    return false;
  }

  get isLoggedIn() {
    return !!this.user;
  }

  login = async (creds: UserFormValues) => {
    try {
      const user = await agent.Account.login(creds);
      store.commonStore.setToken(user.token);
      this.startRefreshTokenTimer(user);
      runInAction(() => {
        this.user = user;
        // this.userFormValues = { ...creds }; // Set the email in userFormValues
      });
      router.navigate("/activities");
      store.modalStore.closeModal();
      console.log(user);
    } catch (error) {
      console.log(error);
      throw error;
    }
  };

  regsiter = async (creds: UserFormValues) => {
    try {
      const user = await agent.Account.regsiter(creds);
      store.commonStore.setToken(user.token);
      this.startRefreshTokenTimer(user);
      runInAction(() => (this.user = user));
      router.navigate("/activities");
      store.modalStore.closeModal();
      console.log(user);
    } catch (error) {
      console.log(error);
      throw error;
    }
  };

  logout = () => {
    store.commonStore.setToken(null);
    this.user = null;
    router.navigate("/");
  };

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

  setImage = (image: string) => {
    if (this.user) this.user.image = image;
  };

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

  private startRefreshTokenTimer(user: User) {
    const jwtToken = JSON.parse(atob(user.token.split(".")[1]));
    const expires = new Date(jwtToken.exp * 1000);
    console.log("expires ",expires );
    const timeout = expires.getTime() - Date.now() - 60 * 1000;
    console.log("timeout ",timeout );
    this.refreshTokenTimeout = setTimeout(this.refreshToken, timeout);
    console.log("refreshTimeout",{ refreshTimeout: this.refreshTokenTimeout });
  }

  private stopRefreshTokenTimer() {
    clearTimeout(this.refreshTokenTimeout);
    
  }
}
