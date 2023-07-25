import React from 'react'
import { useField } from 'formik';
import { Form,Label } from 'semantic-ui-react';
import { ReactJewishDatePicker, ReactJewishDatePickerProps} from "react-jewish-datepicker";
import { BasicJewishDay } from 'jewish-dates-core';
require("react-jewish-datepicker/dist/index.css");

export default function MyHebrewDateInput(props: Partial<ReactJewishDatePickerProps>){
    const [field,meta,helpers] = useField(props.className!)
    return(
        <Form.Field>
            <ReactJewishDatePicker 
            {...field}
            {...props}
            onClick={function (startDay: BasicJewishDay, endDay: BasicJewishDay): void {
                helpers.setValue(field.value)
            } }              />                        
        </Form.Field>
    )   
}