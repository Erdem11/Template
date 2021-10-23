using Template.Common.Models.ModelBase;

namespace Template.Common.Models.Identity.Requests
{
    public class LoginRequest : IRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}