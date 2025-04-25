// Import necessary modules and components
import { observer } from "mobx-react-lite"; // Observer for MobX state management
import { useStore } from "../../app/stores/store"; // Custom hook to access the store
import { Container, Header, Segment } from "semantic-ui-react"; // UI components

// ServerError component displays server error details
export default observer(function ServerError() {
  const { commonStore } = useStore(); // Access commonStore from the MobX store
  return (
    <Container>
      {" "}
      {/* Main container for the error message */}
      <Header as="h1" content="Server Error" /> {/* Main error header */}
      <Header
        sub
        as="h5"
        color="red"
        content={commonStore.error?.message}
      />{" "}
      {/* Subheader for error message */}
      {commonStore.error
        ?.detalis /* Conditional rendering for error details */ && (
        <Segment>
          {" "}
          {/* Segment to display stack trace */}
          <Header as="h4" content="Stuck trace" color="teal" />{" "}
          {/* Header for stack trace */}
          <code style={{ marginTop: "10px" }}>
            {commonStore.error.detalis}
          </code>{" "}
          {/* Display stack trace */}
        </Segment>
      )}
    </Container>
  );
});
