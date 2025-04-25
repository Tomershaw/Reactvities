// Import necessary modules and components
import { useState } from "react"; // React hook for state management
import { Button, Header, Segment } from "semantic-ui-react"; // UI components
import axios from "axios"; // HTTP client for API requests
import ValidationError from "./ValidationError"; // Component to display validation errors

// TestErrors component for testing various error scenarios
export default function TestErrors() {
  const [errors, setErrors] = useState(null); // State to store validation errors

  function handleNotFound() {
    axios.get("/buggy/not-found").catch(err => console.log(err.response)); // Handle 404 error
  }

  function handleBadRequest() {
    axios.get("/buggy/bad-request").catch(err => console.log(err.response)); // Handle 400 error
  }

  function handleServerError() {
    axios.get("/buggy/server-error").catch(err => console.log(err.response)); // Handle 500 error
  }

  function handleUnauthorised() {
    axios.get("/buggy/unauthorised").catch(err => console.log(err.response)); // Handle 401 error
  }

  function handleBadGuid() {
    axios.get("/activities/notaguid").catch(err => console.log(err.response)); // Handle invalid GUID error
  }

  function handleValidationError() {
    axios.post("/activities", {}).catch(err => setErrors(err)); // Handle validation error and update state
  }

  return (
    <>
      <Header as="h1" content="Test Error component" /> {/* Component header */}
      <Segment>
        {" "}
        {/* Container for buttons */}
        <Button.Group widths="7">
          {" "}
          {/* Grouped buttons for error testing */}
          <Button onClick={handleNotFound} content="Not Found" basic primary />
          <Button
            onClick={handleBadRequest}
            content="Bad Request"
            basic
            primary
          />
          <Button
            onClick={handleValidationError}
            content="Validation Error"
            basic
            primary
          />
          <Button
            onClick={handleServerError}
            content="Server Error"
            basic
            primary
          />
          <Button
            onClick={handleUnauthorised}
            content="Unauthorised"
            basic
            primary
          />
          <Button onClick={handleBadGuid} content="Bad Guid" basic primary />
        </Button.Group>
      </Segment>
      {errors && <ValidationError errors={errors} />}{" "}
      {/* Display validation errors if present */}
    </>
  );
}
