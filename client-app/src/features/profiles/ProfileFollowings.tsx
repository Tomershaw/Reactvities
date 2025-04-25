// Import necessary modules and components
import { Tab, Grid, Header, Card } from "semantic-ui-react";
import ProfileCard from "./ProfileCard";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";

// React component to display the list of followings or followers in a tab pane
export default observer(function ProfileFollowings() {
  const { profileStore } = useStore(); // Access the profile store from MobX
  const { profile, followings, loadingFollowings, activeTab } = profileStore; // Destructure necessary data from the store

  return (
    // Render a tab pane with a loading indicator
    <Tab.Pane loading={loadingFollowings}>
      <Grid>
        <Grid.Column width="16">
          <Header
            floated="left" // Align the header to the left
            icon="user" // Display a user icon
            content={
              activeTab === 3
                ? `People following ${profile!.displayName}` // Header for followers
                : `People ${profile?.displayName} is following`
            } // Header for followings
          />
        </Grid.Column>
        <Grid.Column width="16">
          <Card.Group itemsPerRow="5">
            {" "}
            {/* Display profiles in a grid of cards */}
            {followings.map(profile => (
              <ProfileCard key={profile.userName} profile={profile} /> // Render a card for each profile
            ))}
          </Card.Group>
        </Grid.Column>
      </Grid>
    </Tab.Pane>
  );
});
