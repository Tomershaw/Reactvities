// Import necessary modules and components from React Router and Semantic UI
import { Link } from "react-router-dom";
import { Button, Header, Icon, Segment } from "semantic-ui-react";

// NotFound component displays a user-friendly message when a page is not found
export default function NotFound() {
  return (
    <Segment placeholder>
      {" "}
      {/* Placeholder segment for styling */}
      <Header icon>
        {" "}
        {/* Header with an icon */}
        <Icon name="search" /> {/* Search icon to indicate a missing page */}
        Oops- we've looked everywhere but could not find what you are looking
        for! {/* Error message */}
      </Header>
      <Segment.Inline>
        {" "}
        {/* Inline segment for button alignment */}
        <Button as={Link} to="/activities">
          {" "}
          {/* Button to navigate back to activities page */}
          Return to activities page
        </Button>
      </Segment.Inline>
    </Segment>
  );
}
