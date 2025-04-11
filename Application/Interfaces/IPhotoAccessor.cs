using Application.Photos;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    // Interface that abstracts photo storage operations.
    // Allows for uploading and deleting photos regardless of the storage provider (e.g., Cloudinary).

    public interface IPhotoAccessor
    {
        // Uploads a photo to the storage provider and returns the result (URL and public ID).
        Task<PhotoUploadResult> AddPhoto(IFormFile file);

        // Deletes a photo from the storage provider using its public ID.
        Task<string> DeletePhoto(string publicId);
    }
}
