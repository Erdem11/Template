using System;
using Template.Common.Structs;

namespace Template.Domain.Dto.Abstract
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
        Guid Id { get; set; }
    }

    public class EntityBase : IEntityBase
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}