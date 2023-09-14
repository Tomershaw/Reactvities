import { makeAutoObservable, runInAction } from "mobx";
import { User, UserFormValues } from "../models/user";
import agent from "../api/agent";
import { store } from "./store";
import { router } from "../router/Routes";

export default class UserStore {
  user: User | null = null;
  userFormValues: UserFormValues | null = null;
  fbLoading = false;

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
        runInAction(() => {
            this.user = user;
            this.fbLoading = false;
        })
        router.navigate('/activities');
    } catch (error) {
        console.log(error);
        runInAction(() => this.fbLoading = false);
    }
}
}
