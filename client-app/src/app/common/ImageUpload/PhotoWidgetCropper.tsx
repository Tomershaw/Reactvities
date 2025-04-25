import { Cropper } from 'react-cropper';
import 'cropperjs/dist/cropper.css';

interface Props {
  imagePreview: string; // Image preview URL to be cropped
  setCropper: (cropper: Cropper) => void; // Callback to pass cropper instance to parent
}

/**
 * PhotoWidgetCropper
 * 
 * A simple image cropping component using Cropper.js.
 * Displays a square (1:1) cropping area with a live preview.
 */
export default function PhotoWidgetCropper({ imagePreview, setCropper }: Props) {
  return (
    <Cropper
      src={imagePreview}                          // Image source
      style={{ height: 200, width: '100%' }}      // Cropper size
      initialAspectRatio={1}                      // Initial square ratio
      aspectRatio={1}                             // Fixed 1:1 aspect ratio
      preview=".img-preview"                      // Linked live preview area
      guides={false}                              // Hide crop box guides
      viewMode={1}                                // Keep crop box within canvas
      autoCropArea={1}                            // Fill crop area to max
      background={false}                          // No background overlay
      onInitialized={cropper => setCropper(cropper)} // Expose cropper instance
    />
  );
}
