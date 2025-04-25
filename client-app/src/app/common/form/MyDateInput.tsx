// Import necessary hooks and components from Formik, Semantic UI, and React DatePicker
import { useField } from "formik";
import { Form, Label } from "semantic-ui-react";
import DatePicker, { ReactDatePickerProps } from "react-datepicker";

// Define a custom date input component for Formik forms
export default function MyDateInput(props: Partial<ReactDatePickerProps>) {
  // Use Formik's useField hook to manage field state, meta, and helpers
  const [field, meta, helpers] = useField(props.name!);

  return (
    // Render a Semantic UI Form.Field with error handling
    <Form.Field error={meta.touched && !!meta.error}>
      {/* Render the React DatePicker component with Formik field and props */}
      <DatePicker
        {...field}
        {...props}
        selected={(field.value && new Date(field.value)) || null}
        onChange={value => helpers.setValue(value)}
      />

      {/* Display an error message if the field has been touched and contains an error */}
      {meta.touched && meta.error ? (
        <Label basic color="red">
          {meta.error}
        </Label>
      ) : null}
    </Form.Field>
  );
}
