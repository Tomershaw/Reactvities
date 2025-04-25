// This component displays a sidebar with details about attendees of an activity.
// Props:
// - activity: The activity object containing attendees and host information.

import { Segment, List, Label, Item, Image } from "semantic-ui-react";
import { Link } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { Activity } from "../../../app/models/activity";

interface Props {
  activity: Activity; // The activity object to display attendee details for.
}

export default observer(function ActivityDetailedSidebar({
  activity: { attendees, host },
}: Props) {
  if (!attendees) return null; // Return null if there are no attendees.

  return (
    <>
      {/* Header displaying the number of attendees */}
      <Segment
        textAlign="center"
        style={{ border: "none" }}
        attached="top"
        secondary
        inverted
        color="teal"
      >
        {attendees.length} {attendees.length === 1 ? "Person" : "People"} Going
      </Segment>

      {/* List of attendees */}
      <Segment attached>
        <List relaxed divided>
          {attendees.map(attendee => (
            <Item style={{ position: "relative" }} key={attendee.userName}>
              {/* Label indicating the host */}
              {attendee.userName === host?.userName && (
                <Label
                  style={{ position: "absolute" }}
                  color="orange"
                  ribbon="right"
                >
                  Host
                </Label>
              )}

              {/* Attendee avatar */}
              <Image size="tiny" src={attendee.image || "/assets/user.png"} />

              {/* Attendee details */}
              <Item.Content verticalAlign="middle">
                <Item.Header as="h3">
                  <Link to={`/profile/${attendee.userName}`}>
                    {attendee.displayName}
                  </Link>
                </Item.Header>
                {attendee.following && (
                  <Item.Extra style={{ color: "orange" }}>Following</Item.Extra>
                )}
              </Item.Content>
            </Item>
          ))}
        </List>
      </Segment>
    </>
  );
});
