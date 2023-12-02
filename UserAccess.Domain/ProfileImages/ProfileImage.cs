using BuildingBlocks.Domain.Results;

namespace UserAccess.Domain.ProfileImages
{
    public class ProfileImage
    {
        public Guid ProfileImageId { get; set; }
        public string Path { get; set; }
        public string Dimensions { get; set; }
        public string Format { get; set; }

        public ProfileImage(string path, string dimensions, string format)
        {
            ProfileImageId = Guid.NewGuid();
            Path = path;
            Dimensions = dimensions;
            Format = format;
        }

        public static ProfileImage Create(string path, string dimensions, string format)
        {
            return new ProfileImage(path, dimensions, format);
        }

        public static explicit operator ProfileImage(Image v)
        {
            return Create(v.Path, v.Dimensions, v.Format);
        }
    }
}
