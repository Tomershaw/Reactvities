namespace Application.Photos
{
    // Represents the result of a photo upload operation to an external storage provider (e.g., Cloudinary)

    public class PhotoUploadResult
    {
        // Public ID of the uploaded image (used for deletion or reference)
        public string PublicId { get; set; }

        // URL to access the uploaded image
        public string Url { get; set; }
    }
}
