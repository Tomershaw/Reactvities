import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Container} from 'semantic-ui-react';
import { Activity } from '../models/activity';
import NavBar from './NavBar';
import ActivityDashboard from '../../features/activities/dashboard/ActivityDashboard';
import {v4  as uuid} from 'uuid'
import agent from '../api/agent';
import LoadingComponent from './loadingComponent';

function App() {
const [activities,setActivities] = useState<Activity[]>([]);
const [selectedActivity,setSelectedActivity] = useState<Activity| undefined>(undefined);
const [editMode ,setEditMod] =useState(false)
const [loading ,setLoading] =useState(true)
const [submitting ,setSubmitting] =useState(false)


useEffect(()=>{
  agent.Activities.list().then(response =>{
    let activities:Activity[] =[];
    response.forEach(activity => {
      activity.date=activity.date.split('T')[0]
      activities.push(activity)
    });
    setActivities(activities)
    setLoading(false)
  })
},[])

function handleSelcetdActivity(id:string){
  setSelectedActivity(activities.find(x => x.id === id))
}

function handleCancelActivity(){
  setSelectedActivity(undefined)
}

function handleFromClose(){
  setEditMod(false)
}

function handleFormOpen(id?: string){
  id? handleSelcetdActivity(id): handleCancelActivity();
  setEditMod(true)
}

function handleCreateOrEditAtivity(activity:Activity){
  setSubmitting(true);  
  if(activity.id) {
    agent.Activities.update(activity).then(() =>{
      setActivities([...activities.filter(x => x.id !== activity.id),activity])
      setSelectedActivity(activity)
      setEditMod(false)
      setSubmitting(false);
      
    })
  }else{
    activity.id =uuid();
    agent.Activities.create(activity).then(() => {
      setActivities([...activities, activity])
      setSelectedActivity(activity)
      setEditMod(false)
      setSubmitting(false);
    })
  }
 
  
  
 
}

function handleDeleteActivity(id:string){
  setSubmitting(true)
  agent.Activities.delete(id).then(()=> {
    setActivities([...activities.filter(x => x.id !==id)])
    setSubmitting(false)
  })
}

if (loading) return <LoadingComponent content='loading app' />

  return (
    <div >
      <NavBar openform={handleFormOpen}/>
     <Container style={{marginTop:'7em'}}>
      <ActivityDashboard
       activities ={activities}
      selectedActivity ={selectedActivity}
      selectActivity={handleSelcetdActivity}
      cancelSelectActivity={handleCancelActivity}
      editmod={editMode}
      openForm={handleFormOpen}
      closeFrom ={handleFromClose}
      createOrEdit ={handleCreateOrEditAtivity}
      DeleteActivity ={handleDeleteActivity}
      submitting ={submitting}
      />
     </Container>
    </div>
  );
}

export default App;
