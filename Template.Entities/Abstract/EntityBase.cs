﻿// extern alias SystemPrivateCoreLib;
using System;

namespace Template.Entities.Abstract
{
    public interface IEntityBase
    {
        MyKey Id { get; set; }
        DateTime CreatedAt { get; set; }
    }

    public class EntityBase : IEntityBase
    {
        public MyKey Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}