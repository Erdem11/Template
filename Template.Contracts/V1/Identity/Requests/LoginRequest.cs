using Template.Contracts.V1.ModelBase;

namespace Template.Contracts.V1.Identity.Requests
{
    public class LoginRequest : IRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}