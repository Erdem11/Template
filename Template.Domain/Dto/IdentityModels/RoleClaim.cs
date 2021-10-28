﻿using System;
using Microsoft.AspNetCore.Identity;
using Template.Common.Structs;
using Template.Domain.Dto.Abstract;

namespace Template.Domain.Dto.IdentityModels
{
    public class RoleClaim : IdentityRoleClaim<MyKey>, ICreatedAt
    {
        public DateTime CreatedAt { get; set; }
    }
}