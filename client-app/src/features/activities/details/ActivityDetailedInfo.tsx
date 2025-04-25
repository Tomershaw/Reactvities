// This component displays detailed information about a specific activity.
// Props:
// - activity: The activity object containing details to display.

// Wraps the component with MobX observer to react to observable state changes.
import { observer } from "mobx-react-lite";
import { Segment, Grid, Icon } from "semantic-ui-react";
import { Activity } from "../../../app/models/activity";
import { format } from "date-fns";

interface Props {
  activity: Activity; // The activity object to display details for.
}

export default observer(function ActivityDetailedInfo({ activity }: Props) {
  return (
    <Segment.Group>
      {/* Activity description section */}
      <Segment attached="top">
        <Grid>
          <Grid.Column width={1}>
            <Icon size="large" color="teal" name="info" /> {/* Info icon */}
          </Grid.Column>
          <Grid.Column width={15}>
            <p>{activity.description}</p> {/* Activity description */}
          </Grid.Column>
        </Grid>
      </Segment>

      {/* Activity date and time section */}
      <Segment attached>
        <Grid verticalAlign="middle">
          <Grid.Column width={1}>
            <Icon name="calendar" size="large" color="teal" />{" "}
            {/* Calendar icon */}
          </Grid.Column>
          <Grid.Column width={15}>
            <span>{format(activity.date!, "dd MMM yyyy h:mm aa")}</span>{" "}
            {/* Formatted activity date */}
          </Grid.Column>
        </Grid>
      </Segment>

      {/* Activity venue and city section */}
      <Segment attached>
        <Grid verticalAlign="middle">
          <Grid.Column width={1}>
            <Icon name="marker" size="large" color="teal" />{" "}
            {/* Location icon */}
          </Grid.Column>
          <Grid.Column width={11}>
            <span>
              {activity.venue}, {activity.city}
            </span>{" "}
            {/* Activity venue and city */}
          </Grid.Column>
        </Grid>
      </Segment>
    </Segment.Group>
  );
});
