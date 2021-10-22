namespace Template.Api.Models.ModelBase
{
    public class Error
    {
        public string Message { get; set; }

        public static implicit operator Error(string val)
        {
            return new()
            {
                Message = val
            };
        }

        public override string ToString() => Message;
    }
}