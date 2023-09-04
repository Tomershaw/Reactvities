import { useEffect, useState } from "react";
import { Grid, Loader} from "semantic-ui-react";
import ActivityList from "./ActivityList";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import ActivityFilters from "./ActivityFilters";
import { PagingParams } from "../../../app/models/pagination";
import InfiniteScroll from "react-infinite-scroller";
import ActivityListItemPlaceholder from "./ActivityListItemPlaceholder";



export default observer(function ActivityDashboard() {

    const {activityStore} =useStore();
    const{loadActivities,activityRegistry,setPagingParams,pagination} = activityStore
    const [loadingNext,setLoadingNext] = useState(false); 

    function handelGetNext(){
        setLoadingNext(true);
         setPagingParams(new PagingParams(pagination!.currentPage + 1))
         loadActivities().then(() => setLoadingNext(false))
    }


    // const{getUser}=userStore
    // || previousUser !== userStore.getUser()
    // const [previousUser, setPreviousUser] = useState(getUser); 
    // const [previousUserId, setPreviousUserId] = useState(null); 

    useEffect(()=>{
        // if (userStore.user) loadActivities(); 
        if(activityRegistry.size <= 1 ) loadActivities();
        //  setPreviousUser(getUser);  
        },[loadActivities,activityRegistry.size])
        
        // if (activityStore.loadingInitial && !loadingNext) return <LoadingComponent content='Loading Activities...' />

    return (
        <Grid>
            <Grid.Column width='10'>
                {activityStore.loadingInitial && !loadingNext ? (
                    <>
                     <ActivityListItemPlaceholder />
                     <ActivityListItemPlaceholder />
                    </>
                ) : (
                    <InfiniteScroll
                            pageStart={0}
                            loadMore={handelGetNext}
                            hasMore={!loadingNext && !!pagination && pagination.currentPage < pagination.totalPages}
                            initialLoad={false}
                        >
                            <ActivityList />
                        </InfiniteScroll>
                )}
            
            </Grid.Column>
            <Grid.Column width='6'>
               <ActivityFilters />  
            </Grid.Column>
            <Grid.Column with ={10}>
                <Loader active={loadingNext} />
            </Grid.Column>
        </Grid>
    )

    // <Button 
    // floated="right"
    // content='More...'
    // positive
    // onClick={handelGetNext}
    // loading={loadingNext}
    // disabled ={pagination?.totalPages === pagination?.currentPage}
    
    // />

})