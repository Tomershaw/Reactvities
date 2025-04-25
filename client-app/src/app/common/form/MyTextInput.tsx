// Import necessary hooks and components from Formik and Semantic UI
import { useField } from "formik";
import { Form, Label } from "semantic-ui-react";

// Define the props interface for the custom text input component
interface Props {
  placeholder: string;
  name: string;
  label?: string;
  type?: string;
}

// Define a custom text input component for Formik forms
export default function MyTextInput(props: Props) {
  // Use Formik's useField hook to manage field state and meta
  const [field, meta] = useField(props.name);
  return (
    // Render a Semantic UI Form.Field with error handling
    <Form.Field error={meta.touched && !!meta.error}>
      {/* Render the label for the text input */}
      <label>{props.label}</label>
      {/* Render the input element with Formik field and props */}
      <input {...field} {...props} />
      {/* Display an error message if the field has been touched and contains an error */}
      {meta.touched && meta.error ? (
        <Label basic color="red">
          {meta.error}
        </Label>
      ) : null}
    </Form.Field>
  );
}
