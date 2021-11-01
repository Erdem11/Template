using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Template.Common;
using Template.Common.Structs;
using Template.Contracts.V1;
using Template.Contracts.V1.Books.Reponses;
using Template.Service;

namespace Template.Api.Controllers.V1._1
{
    [ApiController]
    [Produces("application/json")]
    [ApiVersion("1.1")]
    [Route("[controller]/[action]")]
    public class BookController : TemplateControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        public BookController(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Response<BookResponse>), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(query);

            var books = await _bookService.GetNewBooksAsync(10, paginationFilter);

            books.AddRange(books);

            return Ok(books);
        }
    }
}