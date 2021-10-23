using Template.Common.Structs;

namespace Template.Common.Models.ModelBase
{
    public class IdResponse : ResponseBase
    {
        public MyKey Id { get; set; }

        public static IdResponse Create(MyKey key)
        {
            return new()
            {
                Id = key
            };
        }
    }

}