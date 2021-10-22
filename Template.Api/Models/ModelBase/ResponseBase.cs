using System;

namespace Template.Api.Models.ModelBase
{
    public class ResponseBase<TResponse> where TResponse : IResponse
    {
        public TResponse Data { get; set; }
        public Error Error { get; set; }

        public string Status
        {
            get
            {
                if (Data != null)
                {
                    return Error == default ? "Success" : "Completed with error";
                }

                if (Error != default)
                {
                    return "Error";
                }

                throw new Exception("Status exception");
            }
        }
    }
}