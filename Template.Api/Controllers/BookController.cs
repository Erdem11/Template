using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Common;
using Template.Common.Models.Books.Requests;
using Template.Common.Models.ModelBase;
using Template.Common.Structs;
using Template.Entities.Concrete;
using Template.Service;

namespace Template.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        // [Authorize(Roles = "Admin,Editor")]
        public GuidResponse Add(AddBookRequest request)
        {
            var book = _mapper.Map<Book>(request);
            
            book.AddedUserId = HttpContext.GetUserId().GetValueOrDefault();
            _bookService.Add(book);

            return default;
        }

        [HttpGet]
        [Authorize(Policy = "BookViewer")]
        public object GetAll()
        {
            var books = _bookService.GetNewBooks(10, PageHolder.Create(0, 20));

            return books;
        }
    }
}