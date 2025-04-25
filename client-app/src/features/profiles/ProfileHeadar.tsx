// Import necessary modules and components
import {
  Divider,
  Grid,
  GridColumn,
  Header,
  Item,
  Segment,
  Statistic,
} from "semantic-ui-react";
import { Profile } from "../../app/models/profile";
import { observer } from "mobx-react-lite";
import FollowButton from "./FollowButton";

// Define the props for the component
interface Props {
  profile: Profile; // The profile object containing user data
}

// React component to display the profile header with user details and follow button
export default observer(function ProfileHeader({ profile }: Props) {
  return (
    <Segment>
      <Grid>
        <GridColumn width={12}>
          {" "}
          {/* Main section with user image and name */}
          <Item.Group>
            <Item>
              <Item.Image
                avatar
                size="small"
                src={profile.image || "/assets/user.png"}
              />{" "}
              {/* User avatar */}
              <Item.Content verticalAlign="middle">
                <Header as="h1" content={profile.displayName} />{" "}
                {/* User display name */}
              </Item.Content>
            </Item>
          </Item.Group>
        </GridColumn>
        <Grid.Column width={4}>
          {" "}
          {/* Sidebar with statistics and follow button */}
          <Statistic.Group widths={2}>
            {" "}
            {/* Display follower and following counts */}
            <Statistic label="followers" value={profile.followersCount} />
            <Statistic label="following" value={profile.followingCount} />
          </Statistic.Group>
          <Divider /> {/* Divider between statistics and follow button */}
          <FollowButton profile={profile} /> {/* Follow/unfollow button */}
        </Grid.Column>
      </Grid>
    </Segment>
  );
});
