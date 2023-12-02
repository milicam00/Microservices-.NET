namespace BuildingBlocks.Domain.Results
{
    public class ResetResult
    {
        private ResetResult(int resetCode, string email, string errorMessage)
        {
            ResetCode = resetCode;
            Email = email;
            ErrorMessage = errorMessage;
        }
        public int ResetCode { get; set; }
        public string Email { get; set; }
        public string ErrorMessage { get; set; }

        public static ResetResult Succcess(int resetCode, string email)
        {
            return new ResetResult(resetCode, email, null);
        }

        public static ResetResult Failure(string errorMessage)
        {
            return new ResetResult(0, null, errorMessage);
        }

    }
}
