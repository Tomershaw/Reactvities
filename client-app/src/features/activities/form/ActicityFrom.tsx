    import React, { ChangeEvent, useState } from 'react'
    import { Segment,Form, Button } from 'semantic-ui-react'
    import { Activity } from '../../../app/models/activity'

    interface Props{
        activity:Activity |undefined;
        closeFrom: () => void;
        createOrEdit:(activity: Activity) => void;
    }

    export default function ActivityFrom({activity :selectedActivity,closeFrom,createOrEdit}:Props){

        const initialState = selectedActivity ?? {
            id:  '',
            title:'',
            category: '',
            description: '',
            date:'',
            city:'',
            venue:'',

        }
        const [activity, setActivity] =  useState(initialState)


        function HandleSubmit(){
            createOrEdit(activity);

        }

        function HandleInputChange(event:ChangeEvent<HTMLInputElement | HTMLTextAreaElement>){
            const{name,value} =event.target;    
            setActivity({...activity,[name]:value})
        }

 


        return(
            <Segment clearing >
                <Form onSubmit={HandleSubmit} autoComplete='off'>
                    <Form.Input placeholder="Title" value={activity.title}  name="title" onChange={HandleInputChange}  />
                    <Form.TextArea placeholder="Description"  value={activity.description}  name="description" onChange={HandleInputChange} />
                    <Form.Input placeholder="category"  value={activity.category}  name="category" onChange={HandleInputChange} />
                    <Form.Input placeholder="date"  value={activity.date}  name="date" onChange={HandleInputChange} />
                    <Form.Input placeholder="City"  value={activity.city}  name="city" onChange={HandleInputChange} />
                    <Form.Input placeholder="Venue"  value={activity.venue}  name="venue" onChange={HandleInputChange} />
                    <Button floated='right' positive type="submit" content="Submit" />
                    <Button  onClick={closeFrom} floated='right' type="button" content="Cancel"  />
                </Form>
            </Segment>
        )
    }