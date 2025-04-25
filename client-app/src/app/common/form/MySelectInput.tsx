// Import necessary hooks and components from Formik and Semantic UI
import { useField } from "formik";
import { Form, Label, Select } from "semantic-ui-react";

// Define the props interface for the custom select input component
interface Props {
  placeholder: string;
  name: string;
  options: { text: string; value: string }[];
  label?: string;
}

// Define a custom select input component for Formik forms
export default function MySelectInput(props: Props) {
  // Use Formik's useField hook to manage field state, meta, and helpers
  const [field, meta, helper] = useField(props.name);

  return (
    // Render a Semantic UI Form.Field with error handling
    <Form.Field error={meta.touched && !!meta.error}>
      {/* Render the label for the select input */}
      <label>{props.label}</label>
      {/* Render the Semantic UI Select component with Formik field and props */}
      <Select
        clearable
        options={props.options}
        value={field.value || null}
        onChange={(_, d) => helper.setValue(d.value)}
        onBlur={() => helper.setTouched(true)}
        placeholder={props.placeholder}
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
