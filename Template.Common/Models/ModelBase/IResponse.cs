namespace Template.Common.Models.ModelBase
{
    public interface IResponse
    {
        public Error Error { get; set; }
        public bool Success { get; }
    }
}