// Import necessary modules and components
import { Grid } from "semantic-ui-react";
import ProfileHeader from "./ProfileHeadar";
import ProfileContent from "./ProfileContent";
import { observer } from "mobx-react-lite";
import { useStore } from "../../app/stores/store";
import { useEffect } from "react";
import LoadingComponent from "../../app/layout/loadingComponent";
import { useParams } from "react-router-dom";

// React component to display a user's profile page
export default observer(function ProfilePage() {
  const { username } = useParams<{ username: string }>(); // Get the username from the route parameters
  const { profileStore } = useStore(); // Access the profile store from MobX
  const { loadingProfile, loadProfile, profile, setActiveTab } = profileStore; // Destructure necessary data and methods from the store

  useEffect(() => {
    if (username) loadProfile(username); // Load the profile data when the component mounts
    return () => {
      setActiveTab(0); // Reset the active tab when the component unmounts
    };
  }, [loadProfile, username, setActiveTab]);

  if (loadingProfile) return <LoadingComponent content="Loading profile..." />; // Show a loading indicator while the profile is being loaded

  return (
    <Grid>
      <Grid.Column with={16}>
        {" "}
        {/* Main content area */}
        {profile && (
          <>
            <ProfileHeader profile={profile} />{" "}
            {/* Render the profile header */}
            <ProfileContent profile={profile} />{" "}
            {/* Render the profile content */}
          </>
        )}
      </Grid.Column>
    </Grid>
  );
});
