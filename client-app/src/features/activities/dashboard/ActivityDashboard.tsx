import React from "react";
import { Grid} from "semantic-ui-react";
import { Activity } from "../../../app/models/activity";
import ActivityList from "./ActivityList";
import ActivityDetalis from "../details/ActivityDetalis";
import ActivityFrom from "../form/ActicityFrom";

interface Props{
    activities:Activity[];
    selectedActivity: Activity | undefined;
    selectActivity:(id:string) => void;
    cancelSelectActivity:() => void;
    editmod:boolean;    
    openForm:(id:string) => void;
    closeFrom:() => void;
    createOrEdit:(activity: Activity) => void;
    DeleteActivity:(id:string) => void;
}



export default function ActivityDashboard({activities,selectedActivity,selectActivity,cancelSelectActivity
    ,editmod,openForm,closeFrom,createOrEdit,DeleteActivity}:Props) {
    return (
        <Grid>
            <Grid.Column width='10'>
                <ActivityList activities = {activities} selectActivity={selectActivity}  DeleteActivity ={DeleteActivity} />
            </Grid.Column>
            <Grid.Column width='6'>
                {selectedActivity&& !editmod &&
                <ActivityDetalis activity={selectedActivity}
                 cancelSelectActivity ={cancelSelectActivity}
                 openFrom={openForm}
                 />}

                 {editmod  &&
                <ActivityFrom  closeFrom={closeFrom} activity={selectedActivity} createOrEdit={createOrEdit}/>}
            </Grid.Column>

        </Grid>
    )



}