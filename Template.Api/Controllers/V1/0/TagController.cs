using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Caching;
using Template.Caching.RedisCaching;
using Template.Common;
using Template.Common.Structs;
using Template.Contracts.V1;
using Template.Contracts.V1.ModelBase;
using Template.Contracts.V1.Tag;
using Template.Domain.Dto;
using Template.Service;

namespace Template.Api.Controllers.V1._0
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    public class TagController : TemplateControllerBase
    {
        private IMapper _mapper;
        private ITagService _tagService;
        public TagController(IMapper mapper, ITagService tagService)
        {
            _mapper = mapper;
            _tagService = tagService;
        }

        [HttpPost]
        // [Authorize(Roles =
        //     RoleConstants.Admin + "," +
        //     RoleConstants.Editor)]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(IdResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public IActionResult Add(AddTagRequest request)
        {
            var tagLanguages = _mapper.Map<List<TagLanguage>>(request);

            var tag = _tagService.Add(tagLanguages, ContextUserId);

            return Created(tag.Id.ToString(), new IdResponse(tag.Id));
        }

        [HttpGet]
        [Cached(60)]
        // [Authorize(Roles =
        //     RoleConstants.Admin + "," +
        //     RoleConstants.Editor)]
        // [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(IdResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public IActionResult GetAll([FromQuery] PaginationQuery query)
        {
            var filter = _mapper.Map<PaginationFilter>(query);

            var tags = _tagService.GetAllTags(filter);

            return Ok(tags);
        }
    }
}