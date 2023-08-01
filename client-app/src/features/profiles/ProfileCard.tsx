import React from "react"
import { Profile } from "../../app/models/profile";
import { Link } from "react-router-dom";
import { Card,Icon,Image} from "semantic-ui-react";

interface Props {
    profile:Profile;
}

export default function ProfileCard({profile}:Props){
    return(
        <Card as={Link} to={`/profile/${profile.userName}`}>
            <Image src={profile.image || '/assets/user.png'} />
            <Card.Content>
                <Card.Header>{profile.displayName}</Card.Header>
                <Card.Description>bio goes  here</Card.Description>
            </Card.Content>
            <Card.Content extra>
                <Icon name="user" />
                20 followers
            </Card.Content>
        </Card>
    )
}

