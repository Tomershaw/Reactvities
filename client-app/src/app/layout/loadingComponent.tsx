// Import necessary components from Semantic UI React
import { Dimmer, Loader } from "semantic-ui-react";

// Define the props interface for the component
interface Props {
  // Optional: Determines if the loader background is inverted (default: true)
  inverted?: boolean;
  // Optional: Custom text to display with the loader (default: 'Loading...')
  content?: string;
}

// Functional component to display a loading indicator
export default function LoadingComponent({
  inverted = true,
  content = "Loading...",
}: Props) {
  return (
    // Dimmer component to create a background overlay
    <Dimmer active={true} inverted={inverted}>
      {/* Loader component to display a loading spinner with optional content */}
      <Loader content={content} />
    </Dimmer>
  );
}
