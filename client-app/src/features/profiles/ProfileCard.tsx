// Importing necessary modules and components
import { Profile } from "../../app/models/profile"; // Profile model interface
import { Link } from "react-router-dom"; // For navigation
import { Card, Icon, Image } from "semantic-ui-react"; // UI components from Semantic UI
import FollowButton from "./FollowButton"; // Follow button component

// Props interface defining the expected structure of the profile prop
interface Props {
    profile: Profile; // Profile object containing user details
}

// ProfileCard component displays user profile information in a card format
export default (function ProfileCard({ profile }: Props) {
    return (
        // Card component linking to the user's profile page
        <Card as={Link} to={`/profiles/${profile.userName}`}>
            {/* Display user image or a default placeholder */}
            <Image src={profile.image || '/assets/user.png'} />
            <Card.Content>
                {/* Display user's display name */}
                <Card.Header>{profile.displayName}</Card.Header>
                {/* Placeholder for user's bio */}
                <Card.Description>bio goes here</Card.Description>
            </Card.Content>
            <Card.Content extra>
                {/* Display follower count with an icon */}
                <Icon name="user" />
                {profile.followersCount} followers
            </Card.Content>
            {/* Follow button to follow/unfollow the user */}
            <FollowButton profile={profile} />
        </Card>
    );
});

