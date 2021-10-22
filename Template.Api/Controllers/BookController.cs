using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Common;
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
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [Authorize]
        [HttpPost]
        public object Add(Book book)
        {
            book.AddedUserId = HttpContext.GetUserId().GetValueOrDefault();
            _bookService.Add(book);

            return book;
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