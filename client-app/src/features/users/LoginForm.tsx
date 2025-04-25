// Import necessary libraries and components for the form
import { ErrorMessage, Form, Formik } from "formik";
import MyTextInput from "../../app/common/form/MyTextInput";
import { Button, Header, Label } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";

// Define and export the LoginForm component, wrapped with MobX observer for reactivity
export default observer(function LoginForm() {
  // Access the userStore from the MobX store
  const { userStore } = useStore();

  return (
    // Use Formik for form handling and validation
    <Formik
      initialValues={{ email: "", password: "", error: null }} // Set initial form values
      onSubmit={(values, { setErrors }) =>
        // Call the login function from userStore and handle errors
        userStore
          .login(values)
          .catch(() => setErrors({ error: "Invalid email or password" }))
      }
    >
      {({ handleSubmit, isSubmitting, errors }) => (
        // Render the form with Semantic UI styling
        <Form className="ui form" onSubmit={handleSubmit} autoComplete="off">
          <Header
            as="h2"
            content="Login to Reactivities"
            color="teal"
            textAlign="center"
          />
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
              // Display error message if login fails
              <Label
                style={{ marginBottom: 10 }}
                basic
                color="red"
                content={errors.error}
              />
            )}
          />
          <Button
            loading={isSubmitting} // Show loading state while submitting
            positive
            content="Login"
            type="submit"
            fluid
          />
        </Form>
      )}
    </Formik>
  );
});
