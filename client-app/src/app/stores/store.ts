// Provides a React context for accessing MobX stores
import { createContext, useContext } from "react";
import ActivityStore from "./activityStore"; // Manages activity-related state
import CommonStore from "./commonStore"; // Manages common app state
import UserStore from "./userStore"; // Manages user authentication and data
import ModalStore from "./modalStore"; // Manages modal visibility and state
import ProfileStore from "./profileStore"; // Manages user profiles and related actions
import commentStore from "./commentStore"; // Manages comments and related actions

// Interface defining the structure of the store
interface Store {
  activityStore: ActivityStore; // Activity store instance
  commonStore: CommonStore; // Common store instance
  userStore: UserStore; // User store instance
  modalStore: ModalStore; // Modal store instance
  profileStore: ProfileStore; // Profile store instance
  commentStore: commentStore; // Comment store instance
}

// Instantiates and exports the MobX store
export const store: Store = {
  activityStore: new ActivityStore(),
  commonStore: new CommonStore(),
  userStore: new UserStore(),
  modalStore: new ModalStore(),
  profileStore: new ProfileStore(),
  commentStore: new commentStore(),
};

// Creates a React context for the store
export const StoreContext = createContext(store);

// Custom hook for accessing the store context
export function useStore() {
  return useContext(StoreContext);
}
