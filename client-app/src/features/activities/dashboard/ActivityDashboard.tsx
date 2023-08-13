import React, { useEffect, useState } from "react";
import { Grid} from "semantic-ui-react";
import ActivityList from "./ActivityList";

import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import LoadingComponent from "../../../app/layout/loadingComponent";
import ActivityFilters from "./ActivityFilters";



export default observer(function ActivityDashboard() {

    const {activityStore,userStore} =useStore();
    const{loadActivities,activityRegistry} = activityStore
    
    // const{getUser}=userStore
    // || previousUser !== userStore.getUser()

     const [previousUser, setPreviousUser] = useState();  

    useEffect(()=>{
         if (userStore.user != previousUser ){
            loadActivities(); 
         } 
         
        if(activityRegistry.size <= 1 ) loadActivities();
         console.log("sdsd",previousUser);
         console.log("sdsd1",userStore.user);
    
        },[activityStore,userStore.user])
        
        if (activityStore.loadingInitial) return <LoadingComponent content='Loading Activities...' />

    return (
        <Grid>
            <Grid.Column width='10'>
                <ActivityList/>
            </Grid.Column>
            <Grid.Column width='6'>
               <ActivityFilters />
            </Grid.Column>
        </Grid>
    )



})