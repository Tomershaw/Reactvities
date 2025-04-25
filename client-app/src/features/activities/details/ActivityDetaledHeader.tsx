// This component displays the header section of an activity's detailed view.
// Props:
// - activity: The activity object containing details to display.

// Wraps the component with MobX observer to react to observable state changes.
import { observer } from "mobx-react-lite";
import { Button, Header, Item, Segment, Image, Label } from "semantic-ui-react";
import { Activity } from "../../../app/models/activity";
import { Link } from "react-router-dom";
import { format } from "date-fns";
import { useStore } from "../../../app/stores/store";

const activityImageStyle = {
  filter: "brightness(30%)", // Darkens the background image.
};

const activityImageTextStyle = {
  position: "absolute",
  bottom: "5%",
  left: "5%",
  width: "100%",
  height: "auto",
  color: "white", // Styles for text overlay on the image.
};

interface Props {
  activity: Activity; // The activity object to display details for.
}

export default observer(function ActivityDetailedHeader({ activity }: Props) {
  const {
    activityStore: { updateAttendance, loading, cancelActivityToggle },
  } = useStore(); // Access activity store actions and state.

  return (
    <Segment.Group>
      <Segment basic attached="top" style={{ padding: "0" }}>
        {
          activity.isCancelled && (
            <Label
              style={{ position: "absolute", zIndex: 100, left: -14, top: 20 }}
              ribbon
              color="red"
              content="Cancelled"
            />
          ) // Displays a "Cancelled" label if the activity is cancelled.
        }
        <Image
          src={`/assets/categoryImages/${activity.category}.jpg`}
          fluid
          style={activityImageStyle}
        />{" "}
        {/* Activity category image */}
        <Segment style={activityImageTextStyle} basic>
          <Item.Group>
            <Item>
              <Item.Content>
                <Header
                  size="huge"
                  content={activity.title} // Activity title.
                  style={{ color: "white" }}
                />
                <p>{format(activity.date!, "dd MMM yyyy")}</p>{" "}
                {/* Formatted activity date. */}
                <p>
                  Hosted by{" "}
                  <strong>
                    {" "}
                    <Link to={`/profiles/${activity.host?.userName}`}>
                      {activity.host?.displayName}
                    </Link>
                  </strong>{" "}
                  {/* Link to host's profile. */}
                </p>
              </Item.Content>
            </Item>
          </Item.Group>
        </Segment>
      </Segment>
      <Segment clearing attached="bottom">
        {activity.isHost ? (
          <>
            <Button
              color={activity.isCancelled ? "green" : "red"}
              floated="left"
              basic
              content={
                activity.isCancelled
                  ? "Re-activate Activity"
                  : "Cancel Activity"
              } // Toggle activity cancellation.
              onClick={cancelActivityToggle}
              loading={loading}
            />
            <Button
              as={Link}
              disabled={activity.isCancelled}
              to={`/manage/${activity.id}`}
              color="orange"
              floated="right"
            >
              Manage Event {/* Link to manage the activity. */}
            </Button>
          </>
        ) : activity.isGoing ? (
          <Button onClick={updateAttendance} loading={loading}>
            Cancel attendance
          </Button> // Button to cancel attendance.
        ) : (
          <Button
            color="teal"
            disabled={activity.isCancelled}
            onClick={updateAttendance}
            loading={loading}
          >
            Join Activity
          </Button> // Button to join the activity.
        )}
      </Segment>
    </Segment.Group>
  );
});
