using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [ApiVersion( "1.0" )]
    [ApiVersion( "1.1" )]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        public BookController(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Response<GuidResponse>), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [MapToApiVersion("1.0")]
        public IActionResult Add(AddBookRequest request)
        {
            var book = _mapper.Map<Book>(request);

            book.AddedUserId = HttpContext.GetUserId().GetValueOrDefault();
            _bookService.Add(book);

            return Created(book.Id.ToString(), book.Id);
        }

        [HttpGet]
        // [Authorize(Policy = RoleConstants.Admin)]
        [ProducesResponseType(typeof(Response<BookResponse>), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [MapToApiVersion("1.0")]
        public IActionResult GetAll([FromQuery] PaginationQuery query)
        {
            // var paginationFilter = _mapper.Map<PaginationFilter>(query);
            
            var books = _bookService.GetNewBooks(10, new PaginationFilter{PageNo = 0,PageSize = 100});
            
            return Ok(books);
        }
        
        [HttpGet]
        [MapToApiVersion("1.0")]
        public IActionResult GetV()
        {
            return Ok("1");
        }
        
        
        [HttpGet]
        [MapToApiVersion("1.0")]
        public IActionResult Only1()
        {
            return Ok("1");
        }
    }
}