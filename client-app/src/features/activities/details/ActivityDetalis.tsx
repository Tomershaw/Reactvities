// This component displays the detailed view of a specific activity.
// It includes the header, information, chat, and sidebar sections.

import { useEffect } from "react";
import { Grid } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import LoadingComponent from "../../../app/layout/loadingComponent";
import { observer } from "mobx-react-lite";
import { useParams } from "react-router-dom";
import ActivityDetailedHeader from "./ActivityDetaledHeader";
import ActivityDetailedInfo from "./ActivityDetailedInfo";
import ActivityDetailedChat from "./ActivityDetailedChat";
import ActivityDetailedSideBar from "./ActivityDetailedSideBar";

export default observer(function ActivityDetalis() {
  const { activityStore } = useStore(); // Access the activity store from MobX.
  const {
    selectedActivity: activity,
    loadActivity,
    loadingInitial,
    clearSelectedActivity,
  } = activityStore;
  const { id } = useParams<{ id: string }>(); // Extract the activity ID from the route parameters.

  useEffect(() => {
    if (id) loadActivity(id); // Load the activity details if an ID is provided.
    return () => clearSelectedActivity(); // Clear the selected activity on component unmount.
  }, [id, loadActivity, clearSelectedActivity]);

  if (loadingInitial || !activity) return <LoadingComponent />; // Show a loading spinner if data is being fetched.

  return (
    <Grid>
      <Grid.Column width={10}>
        <ActivityDetailedHeader activity={activity} /> {/* Header section */}
        <ActivityDetailedInfo activity={activity} /> {/* Information section */}
        <ActivityDetailedChat activityId={activity.id} /> {/* Chat section */}
      </Grid.Column>
      <Grid.Column width={6}>
        <ActivityDetailedSideBar activity={activity} /> {/* Sidebar section */}
      </Grid.Column>
    </Grid>
  );
});
