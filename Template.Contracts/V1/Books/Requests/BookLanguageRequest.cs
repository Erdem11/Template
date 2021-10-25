using Template.Common.Types;

namespace Template.Contracts.V1.Books.Requests
{
    public class BookLanguageRequest
    {
        public Languages Language { get; set; }
        public string Name { get; set; }
    }
}