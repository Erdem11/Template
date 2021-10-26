using System;
using Microsoft.AspNetCore.Mvc;
using Template.Common;

namespace Template.Api.Controllers
{
    public class TemplateControllerBase : Controller
    {
        private Guid? _contextUserId;
        public Guid ContextUserId => _contextUserId ??= (Guid)HttpContext.GetUserId().GetValueOrDefault();
    }
}