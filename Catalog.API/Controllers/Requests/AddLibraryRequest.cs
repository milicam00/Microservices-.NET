namespace Catalog.API.Controllers.Requests
{
    public class AddLibraryRequest
    {
        public string LibraryName { get; set; } 
        public bool IsActive { get; set; }  
    }
}
