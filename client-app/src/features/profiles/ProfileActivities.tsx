import { SyntheticEvent, useEffect } from "react";
import { observer } from "mobx-react-lite";
import { Tab, Grid, Header, Card, Image, TabProps } from "semantic-ui-react";
import { Link } from "react-router-dom";
import { UserActivity } from "../../app/models/profile";
import { format } from "date-fns";
import { useStore } from "../../app/stores/store";

const panes = [
  { menuItem: "Future Events", pane: { key: "future" } },
  { menuItem: "Past Events", pane: { key: "past" } },
  { menuItem: "Hosting", pane: { key: "hosting" } },
];

// This component displays a user's activities categorized into tabs (Future Events, Past Events, Hosting).
// It uses MobX for state management and Semantic UI for UI components.

// Observer wrapper to react to MobX state changes.
export default observer(function ProfileActivities() {
  const { profileStore } = useStore(); // Access MobX store.
  const {
    loadUserActivities, // Function to load user activities based on category.
    profile, // Current user profile.
    loadingActivities, // Loading state for activities.
    userActivities, // List of user activities.
  } = profileStore;

  useEffect(() => {
    loadUserActivities(profile!.userName); // Load activities for the current user on component mount.
  }, [loadUserActivities, profile]);

  const handleTabChange = (_: SyntheticEvent, data: TabProps) => {
    loadUserActivities(
      profile!.userName,
      panes[data.activeIndex as number].pane.key
    ); // Load activities for the selected tab.
  };

  return (
    <Tab.Pane loading={loadingActivities}>
      {" "}
      {/* Show loading spinner while activities are loading. */}
      <Grid>
        <Grid.Column width={16}>
          <Header floated="left" icon="calendar" content={"Activities"} />{" "}
          {/* Header for activities section. */}
        </Grid.Column>
        <Grid.Column width={16}>
          <Tab
            panes={panes} // Tab options for activity categories.
            menu={{ secondary: true, pointing: true }} // Tab menu styling.
            onTabChange={(e, data) => handleTabChange(e, data)} // Handle tab change event.
          />
          <br />
          <Card.Group itemsPerRow={4}>
            {" "}
            {/* Display activities in a grid of cards. */}
            {userActivities.map((activity: UserActivity) => (
              <Card
                as={Link} // Link to activity details page.
                to={`/activities/${activity.id}`}
                key={activity.id}
              >
                <Image
                  src={`/assets/categoryImages/${activity.category}.jpg`} // Activity category image.
                  style={{ minHeight: 100, objectFit: "cover" }}
                />
                <Card.Content>
                  <Card.Header textAlign="center">{activity.Title}</Card.Header>{" "}
                  {/* Activity title. */}
                  <Card.Meta textAlign="center">
                    <div>
                      {activity.Date
                        ? format(new Date(activity.Date), "do LLL")
                        : "Date Not Available"}{" "}
                      {/* Formatted activity date. */}
                    </div>
                    <div>
                      {activity.Date
                        ? format(new Date(activity.Date), "h:mm a")
                        : "Time Not Available"}{" "}
                      {/* Formatted activity time. */}
                    </div>
                  </Card.Meta>
                </Card.Content>
              </Card>
            ))}
          </Card.Group>
        </Grid.Column>
      </Grid>
    </Tab.Pane>
  );
});
