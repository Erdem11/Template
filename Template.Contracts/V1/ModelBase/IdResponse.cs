using System;

namespace Template.Contracts.V1.ModelBase
{
    public class IdResponse
    {
        public Guid Id { get; set; }
        public IdResponse()
        {

        }
        
        public IdResponse(Guid id)
        {
            Id = id;

        }

        public static IdResponse Create(Guid id)
        {
            return new IdResponse
            {
                Id = id
            };
        }
        
        public static implicit operator IdResponse(Guid id)
        {
            return new IdResponse
            {
                Id = id
            };
        }
    }
}