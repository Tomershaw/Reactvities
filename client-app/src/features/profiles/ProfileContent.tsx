// Import necessary modules and components
import { Tab } from "semantic-ui-react";
import ProfilePhotos from "./ProfilePhotos";
import { Profile } from "../../app/models/profile";
import { observer } from "mobx-react-lite";
import ProfileFollowings from "./ProfileFollowings";
import { useStore } from "../../app/stores/store";
import ProfileActivities from "./ProfileActivities";

// Define the props for the component
interface Props {
  profile: Profile; // The profile object containing user data
}

// React component to display profile content with tabs
export default observer(function ProfileContent({ profile }: Props) {
  const { profileStore } = useStore(); // Access the profile store from MobX

  // Define the tabs and their respective content
  const panes = [
    { menuItem: "About", render: () => <Tab.Pane>About Content</Tab.Pane> },
    { menuItem: "Photos", render: () => <ProfilePhotos profile={profile} /> },
    { menuItem: "Events", render: () => <ProfileActivities /> },
    { menuItem: "Followers", render: () => <ProfileFollowings /> },
    { menuItem: "Following", render: () => <ProfileFollowings /> },
  ];

  return (
    // Render a vertical tab menu with the defined panes
    <Tab
      menu={{ fluid: true, vertical: true }} // Make the menu fluid and vertical
      menuPosition="right" // Position the menu on the right
      panes={panes} // Pass the defined panes to the Tab component
      onTabChange={(_e, data) =>
        profileStore.setActiveTab(data.activeIndex as number)
      } // Update the active tab in the store
    />
  );
});
