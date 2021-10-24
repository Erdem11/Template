using System;
using Template.Common.Structs;

namespace Template.Entities.Abstract
{
    public interface IEntityBase : IMyKey, ICreatedAt
    {
    }

    public interface ICreatedAt
    {
        DateTime CreatedAt { get; set; }
    }

    public interface IMyKey
    {
        MyKey Id { get; set; }
    }

    public class EntityBase : IEntityBase
    {
        public MyKey Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}