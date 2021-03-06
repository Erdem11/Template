using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Caching;
using Template.Common;
using Template.Common.Structs;
using Template.Contracts.V1;
using Template.Contracts.V1.Books.Reponses;
using Template.Contracts.V1.Books.Requests;
using Template.Contracts.V1.ModelBase;
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
    public class BookController : TemplateControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        public BookController(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles =
            RoleConstants.Admin + "," +
            RoleConstants.Editor)]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(Response<GuidResponse>), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Add(AddBookRequest request)
        {
            var book = _mapper.Map<Book>(request);

            book.AddedUserId = HttpContext.GetUserId().GetValueOrDefault();
            await _bookService.AddAsync(book, true);

            return Created(book.Id.ToString(), book.Id);
        }

        [HttpGet]
        [Cached(60)]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(Response<BookResponse>), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(query);

            var books = await _bookService.GetNewBooksAsync(10, paginationFilter);

            return Ok(books);
        }
    }
}