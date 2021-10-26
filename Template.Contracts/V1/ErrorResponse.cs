namespace Template.Contracts.V1
{
    public class ErrorResponse
    {
        public Error Error { get; set; }
        public ErrorResponse()
        {

        }
        public ErrorResponse(Error error)
        {
            Error = error;
        }

        public ErrorResponse(string message, int code = -1)
        {
            Error = new Error(message, code);
        }

        public static implicit operator ErrorResponse(Error error)
        {
            return new ErrorResponse
            {
                Error = error
            };
        }
        
        public static implicit operator ErrorResponse((string message, int code) errorTuple)
        {
            var (message, code) = errorTuple;
            return new ErrorResponse(message, code);
        }
        
    }
    
    public class Error
    {
        public string Message { get; set; }
        public int Code { get; set; }

        public Error()
        {

        }
        
        public Error(string message, int code = -1)
        {
            Message = message;
            Code = code;
        }
    }

    public class OkResponse
    {
        public string Status { get; set; }
        public OkResponse()
        {
            
        }
        
        public OkResponse(bool isSuccess)
        {
            Status = isSuccess ? "Success" : "Error";
        }
    }
}