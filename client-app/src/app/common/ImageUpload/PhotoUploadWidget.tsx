/**
 * PhotoUploadWidget.tsx
 * 
 * A three-step photo upload component allowing the user to:
 * 1. Upload an image
 * 2. Resize/crop the image using Cropper.js
 * 3. Preview the cropped image and upload it
 * 
 * Props:
 * - loading: boolean - indicates whether an upload operation is in progress
 * - uploadPhoto: (file: Blob) => void - callback to handle the final cropped image blob
 */

import { useEffect, useState } from 'react';
import { Button, Grid, Header } from 'semantic-ui-react';
import PhotoWidgetDropzone from './PhotoWidgetDropzone';
import PhotoWidgetCropper from './PhotoWidgetCropper';

interface Props {
  loading: boolean;
  uploadPhoto: (file: Blob) => void;
}

export default function PhotoUploadWidget({ uploadPhoto, loading }: Props) {
  // Holds uploaded image files with preview URLs
  const [files, setFiles] = useState<object & { preview?: string }[]>([]);

  // Holds a reference to the Cropper instance used for cropping
  const [cropper, setCropper] = useState<Cropper>();

  /**
   * Called when the user confirms cropping.
   * Extracts the cropped image from the canvas and passes it to uploadPhoto.
   */
  function onCrop() {
    if (cropper) {
      cropper.getCroppedCanvas().toBlob(blob => uploadPhoto(blob!));
    }
  }

  /**
   * Cleanup effect:
   * Revokes preview URLs when the component unmounts or the files array changes.
   */
  useEffect(() => {
    return () => {
      files.forEach((file: object & { preview?: string }) =>
        URL.revokeObjectURL(file.preview!)
      );
    };
  }, [files]);

  return (
    <Grid>
      {/* Step 1: Dropzone for image upload */}
      <Grid.Column width={4}>
        <Header sub color="teal" content="Step 1 - Add Photo" />
        <PhotoWidgetDropzone setFiles={setFiles} />
      </Grid.Column>

      <Grid.Column width={1} />

      {/* Step 2: Image cropping tool */}
      <Grid.Column width={4}>
        <Header sub color="teal" content="Step 2 - Resize image" />
        {files && files.length > 0 && (
          <PhotoWidgetCropper
            setCropper={setCropper}
            imagePreview={files[0].preview!}
          />
        )}
      </Grid.Column>

      <Grid.Column width={1} />

      {/* Step 3: Preview cropped image and upload */}
      <Grid.Column width={4}>
        <Header sub color="teal" content="Step 3 - Preview & Upload" />
        {files && files.length > 0 && (
          <>
            <div className="img-preview" style={{ minHeight: 200, overflow: 'hidden' }} />
            <Button.Group widths={2}>
              <Button loading={loading} onClick={onCrop} positive icon="check" />
              <Button disabled={loading} onClick={() => setFiles([])} icon="close" />
            </Button.Group>
          </>
        )}
      </Grid.Column>
    </Grid>
  );
}
