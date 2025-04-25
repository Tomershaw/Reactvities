import { makeAutoObservable, reaction, runInAction } from "mobx";
import Photo, { Profile, UserActivity } from "../models/profile";
import agent from "../api/agent";
import { store } from "./store";

// MobX store for managing user profiles and related actions
export default class ProfileStore {
  profile: Profile | null = null; // Current user's profile
  loadingProfile = false; // Loading state for profile
  uploading = false; // Loading state for photo uploads
  loading = false; // General loading state
  followings: Profile[] = []; // List of profiles the user is following
  loadingFollowings = false; // Loading state for followings
  activeTab: number = 0; // Active tab index in the UI
  userActivities: UserActivity[] = []; // List of user activities
  loadingActivities = false; // Loading state for activities

  constructor() {
    makeAutoObservable(this); // Makes the store observable for MobX

    // Reacts to changes in the active tab and loads followings if necessary
    reaction(
      () => this.activeTab,
      activeTab => {
        if (activeTab === 3 || activeTab === 4) {
          const predicate = activeTab === 3 ? "followers" : "following";
          this.loadFollowings(predicate);
        } else {
          this.followings = [];
        }
      }
    );
  }

  // Sets the active tab index
  setActiveTab = (activeTab: number) => {
    this.activeTab = activeTab;
  };

  // Checks if the current profile belongs to the logged-in user
  get isCurrentUser() {
    if (store.userStore.user && this.profile) {
      return store.userStore.user.username === this.profile.userName;
    }
    return false;
  }

  // Loads a user's profile by username
  loadProfile = async (username: string) => {
    this.loadingProfile = true;
    try {
      const profile = await agent.Profiles.get(username);
      runInAction(() => {
        this.profile = profile;
        this.loadingProfile = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => (this.loadingProfile = false));
    }
  };

  // Uploads a photo and updates the profile
  uploadPhoto = async (file: Blob) => {
    this.uploading = true;
    try {
      const response = await agent.Profiles.uploadPhoto(file);
      const photo = response.data;
      runInAction(() => {
        if (this.profile) {
          this.profile.photos?.push(photo);
          if (photo.isMain && store.userStore.user) {
            store.userStore.setImage(photo.url);
            this.profile.image = photo.url;
          }
        }
        this.uploading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => (this.uploading = false));
    }
  };

  // Sets a photo as the main profile picture
  setMainPhoto = async (photo: Photo) => {
    this.loading = true;
    try {
      await agent.Profiles.setMainPhoto(photo.id);
      store.userStore.setImage(photo.url);
      runInAction(() => {
        if (this.profile && this.profile.photos) {
          this.profile.photos.find(p => p.isMain)!.isMain = false;
          this.profile.photos.find(p => p.id === photo.id)!.isMain = true;
          this.profile.image = photo.url;
          this.loading = false;
        }
      });
    } catch (error) {
      runInAction(() => (this.loading = false));
    }
  };

  // Deletes a photo from the profile
  deletePhoto = async (photo: Photo) => {
    this.loading = true;
    await agent.Profiles.deletePhoto(photo.id);
    try {
      runInAction(() => {
        if (this.profile)
          this.profile.photos = this.profile.photos?.filter(
            p => p.id !== photo.id
          );
        this.loading = false;
      });
    } catch (error) {
      runInAction(() => {
        this.loading = false;
        console.log(error);
      });
    }
  };

  // Updates the following status of a user
  updateFollowing = async (username: string, following: boolean) => {
    this.loading = true;
    try {
      await agent.Profiles.updateFollowing(username);
      store.activityStore.updateAttendeeFollowing(username);
      runInAction(() => {
        if (
          this.profile &&
          this.profile.userName !== store.userStore.user?.username &&
          this.profile.userName === username
        ) {
          following
            ? this.profile.followersCount++
            : this.profile.followersCount--;
          this.profile.following = !this.profile.following;
        }
        if (
          this.profile &&
          this.profile.userName === store.userStore.user?.username
        ) {
          following
            ? this.profile.followingCount++
            : this.profile.followingCount--;
        }
        this.followings.forEach(profile => {
          if (profile.userName === username) {
            profile.following
              ? profile.followersCount--
              : profile.followersCount++;
            profile.following = !profile.following;
          }
        });
        this.loading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => (this.loading = false));
    }
  };

  // Loads the list of followings based on a predicate
  loadFollowings = async (predicate: string) => {
    this.loadingFollowings = true;
    try {
      const followings = await agent.Profiles.ListFollowing(
        this.profile!.userName,
        predicate
      );
      runInAction(() => {
        this.followings = followings;
        this.loadingFollowings = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => (this.loadingFollowings = false));
    }
  };

  // Loads user activities with an optional predicate
  loadUserActivities = async (username: string, predicate?: string) => {
    this.loadingActivities = true;
    try {
      const activities = await agent.Profiles.listActivities(
        username,
        predicate!
      );
      runInAction(() => {
        this.userActivities = activities;
        this.loadingActivities = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => {
        this.loadingActivities = false;
      });
    }
  };
}
