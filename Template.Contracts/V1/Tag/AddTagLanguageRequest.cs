using Template.Common.Types;

namespace Template.Contracts.V1.Tag
{
    public class AddTagLanguageRequest
    {
        public string Name { get; set; }
        public Languages Language { get; set; }
    }
}