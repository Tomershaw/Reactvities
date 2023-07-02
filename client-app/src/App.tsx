import React, { useEffect, useState } from 'react';
import logo from './logo.svg';
import './App.css';
import axios from 'axios';
import { Header, List } from 'semantic-ui-react';

function App() {
const [activities,setActivities] = useState([]);

useEffect(()=>{
  axios.get('http://localhost:5000/api/Activities')
  .then(response =>{
    setActivities(response.data)
  })
},[])

  return (
    <div >
      <Header as = 'h2' icon='user' content='Reactivities'/>
        <List>
          {activities.map((activities:any) =>(
            <List.Item key={activities.id}>
              {activities.title}
            </List.Item>
          ))}
        </List>

     
    </div>
  );
}

export default App;
