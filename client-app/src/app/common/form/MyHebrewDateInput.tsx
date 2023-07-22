import React from 'react'
import { useField } from 'formik';
import { Form,Label } from 'semantic-ui-react';
import DatePicker,{ReactDatePickerProps} from 'react-datepicker';
import { ReactJewishDatePicker, ReactJewishDatePickerProps} from "react-jewish-datepicker";
require("react-jewish-datepicker/dist/index.css");




export default function MyHebrewDateInput(props: Partial<ReactJewishDatePickerProps>){
    const [field,meta,helpers] = useField(props.className!)
    return(
        <Form.Field error={meta.touched && !!meta.error}>
            <ReactJewishDatePicker
            {...field}
            {...props}
            value={(field.value && new Date(field.value)) ||null}
            onClick={value =>helpers.setValue(value)}
              />
                         
            {meta.touched && meta.error ? (
                <Label basic color='red'>{meta.error}</Label>
            ):null}
        </Form.Field>

    )
   
}   


