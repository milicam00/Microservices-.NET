namespace Catalog.API.Controllers.Requests
{
    public class ChangeBookRequest
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public int Pages { get; set; }
        public int PubblicationYear { get; set; }
        public double UserRating { get; set; }
        public int NumOfCopies { get; set; }
    }
}
