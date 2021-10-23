using Template.Common.Models.Identity.Responses;

namespace Template.Common.Models.ModelBase
{
    public class ResponseBase : IResponse
    {
        public bool Success => !Error.IsError(Error);
        public Error Error { get; set; }

        public static T ErrorResponse<T>(Error error) where T : ResponseBase, new()
        {
            return new T
            {
                Error = error
            };
        }
    }
}