import { SyntheticEvent } from "react";
import { observer } from "mobx-react-lite";
import { Button, Reveal } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { Profile } from "../../app/models/profile";

// This component renders a follow/unfollow button for a user profile.
// It uses MobX for state management and Semantic UI for styling.

// Props:
// - profile: The profile of the user to follow/unfollow.

interface Props {
  profile: Profile;
}

// Observer wrapper to react to MobX state changes.
export default observer(function FollowButton({ profile }: Props) {
  const { profileStore, userStore } = useStore(); // Access MobX stores.
  const { updateFollowing, loading } = profileStore; // Destructure methods and state from profileStore.

  // If the logged-in user is viewing their own profile, do not render the button.
  if (userStore.user?.username === profile.userName) return null;

  // Handles follow/unfollow button click.
  function handleFollow(e: SyntheticEvent, username: string) {
    e.preventDefault();
    profile.following
      ? updateFollowing(username, false)
      : updateFollowing(username, true);
  }

  return (
    <Reveal animated="move">
      {/* Visible button state */}
      <Reveal.Content visible style={{ width: "100%" }}>
        <Button
          fluid
          color="teal"
          content={profile.following ? "Following" : "Not Following"}
        />
      </Reveal.Content>
      {/* Hidden button state for follow/unfollow action */}
      <Reveal.Content hidden>
        <Button
          loading={loading} // Show loading spinner when updating.
          fluid
          basic
          color={profile.following ? "red" : "green"}
          content={profile.following ? "Unfollow" : "Follow"}
          onClick={e => handleFollow(e, profile.userName)} // Trigger follow/unfollow logic.
        />
      </Reveal.Content>
    </Reveal>
  );
});
