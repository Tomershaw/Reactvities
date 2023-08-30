using System.Runtime.CompilerServices;

namespace API.DTOs
{
    public class FacebookDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public FacebookPicture Picture;
    }

    public class FacebookPicture
    {
        public FacebookPictureData Data { get; set; }
    }

    public class FacebookPictureData
    {
        public int Height { get; set; }
        public string Url { get; set; }
        public bool Is_silhouette { get; set; }
        public int Width { get; set; }

    }
}