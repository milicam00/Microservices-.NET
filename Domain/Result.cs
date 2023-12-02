namespace BuildingBlocks.Domain
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public object Data { get; set; }
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }

        private Result(bool isSuccess, object data, string errorMessage, int statusCode)
        {
            IsSuccess = isSuccess;
            Data = data;
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }

        public static Result Success(object data)
        {
            return new Result(true, data, null, 200);
        }


        public static Result Failure(string errorMessage)
        {
            return new Result(false, null, errorMessage, 400);
        }
    }
}
