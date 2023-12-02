namespace Catalog.Domain.BookImages
{
    public class Image
    {
        public Guid ImageId { get; set; }
        public string Path { get; set; }
        public string Dimensions { get; set; }
        public string Format { get; set; }

        public Image(string path, string dimensions, string format)
        {
            ImageId = Guid.NewGuid();
            Path = path;
            Dimensions = dimensions;
            Format = format;
        }

        public static Image Create(string path, string dimensions, string format)
        {
            return new Image(path, dimensions, format);
        }

        public static explicit operator Image(BuildingBlocks.Domain.Results.Image v)
        {
            return Create(v.Path, v.Dimensions, v.Format);
        }
    }
}
