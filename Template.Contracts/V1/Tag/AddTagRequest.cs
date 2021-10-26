using System.Collections.Generic;

namespace Template.Contracts.V1.Tag
{
    public class AddTagRequest
    {
        public List<AddTagLanguageRequest> Languages { get; set; }
    }
}