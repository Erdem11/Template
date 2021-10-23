namespace Template.Common.Models.ModelBase
{
    public class Error
    {

        public string Message { get; set; }
        public static bool IsError(Error error)
        {
            if (error == default)
                return false;

            if (string.IsNullOrWhiteSpace(error.Message))
                return false;

            return true;
        }

        public static implicit operator Error(string val)
        {
            return new()
            {
                Message = val
            };
        }

        public override string ToString()
        {
            return Message;
        }
    }
}