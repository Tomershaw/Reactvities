using Application.Photos; // Namespace containing the PhotoUploadResult class, which represents the result of a photo upload operation.
using Microsoft.AspNetCore.Http; // Provides access to IFormFile, which represents a file sent with an HTTP request.

namespace Application.Interfaces
{
    // Interface that abstracts photo storage operations.
    // This interface is implemented by classes that interact with a photo storage provider (e.g., Cloudinary).
    // It provides methods for uploading and deleting photos, enabling the application to work with different storage providers seamlessly.

    public interface IPhotoAccessor
    {
        // Uploads a photo to the storage provider.
        // Parameters:
        //   - file: An IFormFile object representing the photo to be uploaded.
        // Returns:
        //   - A Task that resolves to a PhotoUploadResult object containing the URL and public ID of the uploaded photo.
        Task<PhotoUploadResult> AddPhoto(IFormFile file);

        // Deletes a photo from the storage provider.
        // Parameters:
        //   - publicId: A string representing the public ID of the photo to be deleted.
        // Returns:
        //   - A Task that resolves to a string indicating the result of the deletion operation.
        Task<string> DeletePhoto(string publicId);
    }
}
