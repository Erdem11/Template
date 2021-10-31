using System;
using System.Collections.Generic;

namespace Template.Common
{
    public class TokenModel
    {
        public string Sub { get; set; }
        public Guid Jti { get; set; }
        public string Email { get; set; }
        public Guid Id { get; set; }
        public List<string> Roles { get; set; }
        public List<string> CustomClaims { get; set; }
        public int Nbf { get; set; }
        public int Exp { get; set; }
        public int Iat { get; set; }
    }
}