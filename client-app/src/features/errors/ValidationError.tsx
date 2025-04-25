// Import the Message component from Semantic UI for displaying error messages
import { Message } from "semantic-ui-react";

// Define the Props interface for the component
interface Props {
  errors: string[]; // Array of error messages to display
}

// ValidationError component displays a list of validation errors
export default function ValidationError({ errors }: Props) {
  return (
    <Message error>
      {" "}
      {/* Error message container */}
      {errors /* Render the list only if errors exist */ && (
        <Message.List>
          {" "}
          {/* List of error messages */}
          {errors.map(
            (err: string, i: number /* Map each error to a list item */) => (
              <Message.Item key={i}>
                {" "}
                {err}
              </Message.Item> /* Display individual error */
            )
          )}
        </Message.List>
      )}
    </Message>
  );
}
