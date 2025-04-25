using Application.Interfaces;
using Application.Photos;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Photos
{
    public class PhotoAccessor : IPhotoAccessor
    {
        private readonly Cloudinary _cloudinary;
        public PhotoAccessor(IOptions<CloudinarySettings> config)
        {
            // Initialize the Cloudinary instance with the provided account settings.
            var account = new Account (
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
        }

        // Method to upload a photo to Cloudinary.
        public async Task<PhotoUploadResult> AddPhoto(IFormFile file)
        {
            // Check if the file has content.
            if (file.Length > 0)
            {
                // Open a stream to read the file content.
                await using var strem = file.OpenReadStream();

                // Set up the parameters for the image upload, including resizing and cropping.
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, strem),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                };

                // Upload the image to Cloudinary and get the result.
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                // Check for errors in the upload process.
                if (uploadResult.Error != null)
                {
                    throw new Exception(uploadResult.Error.Message);
                }

                // Return the result containing the public ID and URL of the uploaded photo.
                return new PhotoUploadResult
                {
                    PublicId = uploadResult.PublicId,
                    Url = uploadResult.SecureUrl.ToString()
                };
            }

            // Return null if the file is empty.
            return null;
        }

        public async Task<string> DeletePhoto(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result.Result == "ok" ? result.Result: null;
        }
    }
}