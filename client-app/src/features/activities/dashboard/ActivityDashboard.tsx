import React from "react";
import { Grid} from "semantic-ui-react";
import ActivityList from "./ActivityList";
import ActivityDetalis from "../details/ActivityDetalis";
import ActivityFrom from "../form/ActicityFrom";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";


export default observer(function ActivityDashboard() {

    const {activityStore} =useStore();
    const{selectedActivity,editMode} = activityStore;

    return (
        <Grid>
            <Grid.Column width='10'>
                <ActivityList/>
            </Grid.Column>
            <Grid.Column width='6'>
                {selectedActivity&& !editMode &&
                <ActivityDetalis />}

                 {editMode &&
                <ActivityFrom />}
            </Grid.Column>

        </Grid>
    )



})