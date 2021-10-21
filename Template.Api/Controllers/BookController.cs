using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Common;
using Template.Entities.Concrete;
using Template.Service;

namespace Template.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BookController : ControllerBase
    {

        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        [Authorize]
        public object Add(Book book)
        {
            book.AddedUserId = HttpContext.GetUserId().GetValueOrDefault();
            _bookService.Add(book);

            return book;
        }
    }
}