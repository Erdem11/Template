using System.Collections.Generic;

namespace Template.Contracts.V1
{
    public class ValidationErrorResponse
    {
        public string Message { get; set; }
        public List<ValidationError> Errors { get; set; }
    }
    
    public class ValidationError
    {
        public string Key { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}