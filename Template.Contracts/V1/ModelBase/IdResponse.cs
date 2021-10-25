
using System;

namespace Template.Contracts.V1.ModelBase
{
    public class IdResponse
    {
        public Guid Id { get; set; }

        public static IdResponse Create(Guid key)
        {
            return new IdResponse
            {
                Id = key
            };
        }
        public static implicit operator IdResponse(Guid key)
        {
            return new IdResponse
            {
                Id = key
            };
        }
    }

}