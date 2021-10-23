namespace Template.Common.Models.ModelBase
{
    public class EmptyResponse : ResponseBase
    {

        public static EmptyResponse Create()
        {
            return new();
        }
    }
}