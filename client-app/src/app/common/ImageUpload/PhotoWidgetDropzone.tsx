// Import necessary hooks and components
import { useCallback } from "react";
import { useDropzone } from "react-dropzone";
import { Header, Icon } from "semantic-ui-react";

// Define the props interface
interface Props {
  setFiles: (files: object[]) => void; // Function to set the uploaded files
}

// Component for handling image uploads via drag-and-drop
export default function PhotoWidgetDropzone({ setFiles }: Props) {
  // Styles for the dropzone container
  const dzStyle = {
    border: "dashed 3px #eee",
    borderColor: "#eee",
    borderRadius: "5px",
    paddingTop: "30px",
    textAlign: "center",
    height: 200,
  } as object;

  // Styles when a file is being dragged over the dropzone
  const dzActive = {
    borderColor: "green",
  };

  // Handle file drop event
  const onDrop = useCallback(
    (acceptedFiles: object[]) => {
      setFiles(
        acceptedFiles.map((file: object) =>
          Object.assign(file, {
            preview: URL.createObjectURL(file as Blob), // Add a preview URL to each file
          })
        )
      );
      console.log(acceptedFiles); // Log the accepted files
    },
    [setFiles]
  );

  // Extract dropzone props and state
  const { getRootProps, getInputProps, isDragActive } = useDropzone({ onDrop });

  return (
    // Render the dropzone container with dynamic styles
    <div
      {...getRootProps()}
      style={isDragActive ? { ...dzStyle, ...dzActive } : dzStyle}
    >
      <input {...getInputProps()} /> {/* Hidden input for file selection */}
      <Icon name="upload" size="huge" /> {/* Upload icon */}
      <Header content="Drop image here " /> {/* Instructional text */}
    </div>
  );
}
