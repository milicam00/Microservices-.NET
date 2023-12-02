namespace UserAccess.API.Controllers.Requests
{
    public class UploadImageRequest
    {
        public IFormFile FileImage { get; set; }
        public string UserName { get; set; }    
    }
}
