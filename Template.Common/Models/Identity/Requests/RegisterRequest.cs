using Template.Common.Models.ModelBase;

namespace Template.Common.Models.Identity.Requests
{
    public class RegisterRequest : IRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

}