import { useEffect, useState } from "react";
import { Grid, Loader } from "semantic-ui-react";
import ActivityList from "./ActivityList";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import ActivityFilters from "./ActivityFilters";
import { PagingParams } from "../../../app/models/pagination";
import InfiniteScroll from "react-infinite-scroller";
import ActivityListItemPlaceholder from "./ActivityListItemPlaceholder";

// Export the ActivityDashboard component wrapped with observer for MobX reactivity
export default observer(function ActivityDashboard() {
  // Destructure activityStore and its methods from the store context
  const { activityStore } = useStore();
  const { loadActivities, activityRegistry, setPagingParams, pagination } =
    activityStore;
  const [loadingNext, setLoadingNext] = useState(false);

  // Function to handle loading the next page of activities
  function handelGetNext() {
    setLoadingNext(true);
    setPagingParams(new PagingParams(pagination!.currentPage + 1));
    loadActivities().then(() => setLoadingNext(false));
  }

  useEffect(() => {
    // Load activities if the registry is nearly empty
    if (activityRegistry.size <= 1) loadActivities();
  }, [loadActivities, activityRegistry.size]);

  return (
    <Grid>
      <Grid.Column width="10">
        {/* Show placeholders while loading initial activities */}
        {activityStore.loadingInitial && !loadingNext ? (
          <>
            <ActivityListItemPlaceholder />
            <ActivityListItemPlaceholder />
          </>
        ) : (
          // Infinite scrolling for loading more activities
          <InfiniteScroll
            pageStart={0}
            loadMore={handelGetNext}
            hasMore={
              !loadingNext &&
              !!pagination &&
              pagination.currentPage < pagination.totalPages
            }
            initialLoad={false}
          >
            <ActivityList />
          </InfiniteScroll>
        )}
      </Grid.Column>
      <Grid.Column width="6">
        {/* Filters for activity list */}
        <ActivityFilters />
      </Grid.Column>
      <Grid.Column with={10}>
        {/* Loader for indicating next page loading */}
        <Loader active={loadingNext} />
      </Grid.Column>
    </Grid>
  );

  // <Button
  // floated="right"
  // content='More...'
  // positive
  // onClick={handelGetNext}
  // loading={loadingNext}
  // disabled ={pagination?.totalPages === pagination?.currentPage}

  // />
});
