// Import necessary modules and components
import { observer } from "mobx-react-lite";
import { SyntheticEvent, useState } from "react";
import { Card, Header, Tab, Image, Grid, Button } from "semantic-ui-react";
import Photo, { Profile } from "../../app/models/profile";
import { useStore } from "../../app/stores/store";
import PhotoUploadWidget from "../../app/common/ImageUpload/PhotoUploadWidget";

// Define the props for the component
interface Props {
  profile: Profile; // The profile object containing user data
}

// React component to display and manage user photos
export default observer(function ProfilePhotos({ profile }: Props) {
  const {
    profileStore: {
      isCurrentUser,
      uploadPhoto,
      uploading,
      loading,
      setMainPhoto,
      deletePhoto,
    },
  } = useStore(); // Access profile store actions and state
  const [addPhotoMode, setAddPhotoMode] = useState(false); // State to toggle photo upload mode
  const [target, setTarget] = useState(""); // State to track the button triggering an action

  // Handle setting a photo as the main photo
  function handleSetMainPhoto(
    photo: Photo,
    e: SyntheticEvent<HTMLButtonElement>
  ) {
    setTarget(e.currentTarget.name);
    setMainPhoto(photo);
  }

  // Handle deleting a photo
  function handleDeletePhoto(
    photo: Photo,
    e: SyntheticEvent<HTMLButtonElement>
  ) {
    setTarget(e.currentTarget.name);
    deletePhoto(photo);
  }

  // Handle uploading a new photo
  function handlePhotoUpload(file: Blob) {
    uploadPhoto(file).then(() => setAddPhotoMode(false));
  }

  return (
    <Tab.Pane>
      <Grid>
        <Grid.Column width={16}>
          {" "}
          {/* Header section */}
          <Header floated="left" icon="image" content="Photos" />
          {isCurrentUser && (
            <Button
              floated="right"
              basic
              content={addPhotoMode ? "Cancel" : "Add Photo"}
              onClick={() => setAddPhotoMode(!addPhotoMode)} // Toggle photo upload mode
            />
          )}
        </Grid.Column>
        <Grid.Column width={16}>
          {" "}
          {/* Photo display or upload section */}
          {addPhotoMode ? (
            <PhotoUploadWidget
              uploadPhoto={handlePhotoUpload}
              loading={uploading}
            />
          ) : (
            <Card.Group itemsPerRow={5}>
              {" "}
              {/* Display photos in a grid */}
              {profile.photos?.map(photo => (
                <Card key={photo.id}>
                  {" "}
                  {/* Individual photo card */}
                  <Image src={photo.url} />
                  {isCurrentUser && (
                    <Button.Group fluid width={2}>
                      {" "}
                      {/* Action buttons for each photo */}
                      <Button
                        basic
                        color="green"
                        content="Main"
                        name={"main" + photo.id}
                        disabled={photo.isMain} // Disable if already the main photo
                        loading={target === "main" + photo.id && loading}
                        onClick={e => handleSetMainPhoto(photo, e)}
                      />
                      <Button
                        name={photo.id}
                        disabled={photo.isMain} // Disable if the photo is the main photo
                        loading={target === photo.id && loading}
                        onClick={e => handleDeletePhoto(photo, e)}
                        basic
                        color="red"
                        icon="trash"
                      />
                    </Button.Group>
                  )}
                </Card>
              ))}
            </Card.Group>
          )}
        </Grid.Column>
      </Grid>
    </Tab.Pane>
  );
});
