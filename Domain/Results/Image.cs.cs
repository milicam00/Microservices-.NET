namespace BuildingBlocks.Domain.Results
{
    public class Image
    {
        public Guid ImageId { get; set; }
        public string Path { get; set; }
        public string Dimensions { get; set; }
        public string Format { get; set; }
    }
}
