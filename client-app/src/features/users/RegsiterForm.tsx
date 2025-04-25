// Import necessary libraries and components for the form
import { ErrorMessage, Form, Formik } from "formik";
import MyTextInput from "../../app/common/form/MyTextInput";
import { Button, Header } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import * as Yup from "yup";
import ValidationError from "../errors/ValidationError";

// Define and export the RegisterForm component, wrapped with MobX observer for reactivity
export default observer(function RegsiterForm() {
  // Access the userStore from the MobX store
  const { userStore } = useStore();

  return (
    // Use Formik for form handling, validation, and submission
    <Formik
      initialValues={{
        displayName: "",
        username: "",
        email: "",
        password: "",
        error: null,
      }} // Set initial form values
      onSubmit={(values, { setErrors }) =>
        // Call the register function from userStore and handle errors
        userStore.regsiter(values).catch(error => setErrors({ error }))
      }
      validationSchema={Yup.object({
        displayName: Yup.string().required(), // Validate displayName as required
        username: Yup.string().required(), // Validate username as required
        email: Yup.string().required(), // Validate email as required
        password: Yup.string().required(), // Validate password as required
      })}
    >
      {({ handleSubmit, isSubmitting, errors, isValid, dirty }) => (
        // Render the form with Semantic UI styling
        <Form
          className="ui form error"
          onSubmit={handleSubmit}
          autoComplete="off"
        >
          <Header
            as="h2"
            content="Sign up Reactivities"
            color="teal"
            textAlign="center"
          />
          <MyTextInput placeholder="Display name" name="displayName" />{" "}
          {/* Input for display name */}
          <MyTextInput placeholder="User name" name="username" />{" "}
          {/* Input for username */}
          <MyTextInput placeholder="Email" name="email" />{" "}
          {/* Input for email */}
          <MyTextInput
            placeholder="Password"
            name="password"
            type="password"
          />{" "}
          {/* Input for password */}
          <ErrorMessage
            name="error"
            render={() => (
              // Display validation errors if registration fails
              <ValidationError errors={errors.error as unknown as string[]} />
            )}
          />
          <Button
            disabled={!isValid || !dirty || isSubmitting} // Disable button if form is invalid, untouched, or submitting
            loading={isSubmitting} // Show loading state while submitting
            positive
            content="Login" // Button text
            type="submit"
            fluid // Submit button with full width
          />
        </Form>
      )}
    </Formik>
  );
});
