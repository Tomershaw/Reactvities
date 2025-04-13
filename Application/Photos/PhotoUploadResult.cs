namespace Application.Photos
{
    // Represents the result of a photo upload operation to an external storage provider (e.g., Cloudinary)
    // This class is used to encapsulate the response from the photo storage service.

    public class PhotoUploadResult
    {
        // Public ID of the uploaded image (used for deletion or reference)
        // This is a unique identifier assigned by the external storage provider.
        public string PublicId { get; set; }

        // URL to access the uploaded image
        // This is the direct link to the image hosted on the external storage provider.
        public string Url { get; set; }
    }
}
