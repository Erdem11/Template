using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Template.Common.Structs;
using Template.Data;
using Template.Domain.Dto;
using Template.Service.Helpers;

namespace Template.Service
{
    public interface IBookService : IServiceBase<Book>
    {
        Task<List<Book>> GetNewBooksAsync(int day, PaginationFilter pager);
    }

    public class BookService : ServiceBase<Book>, IBookService
    {
        public BookService(TemplateContext context) : base(context)
        {
        }

        public async Task<List<Book>> GetNewBooksAsync(int day, PaginationFilter pageHolder)
        {
            var minimumDate = DateTime.Now.Subtract(new TimeSpan(day, 0, 0));

            Expression<Func<Book, bool>> expression =
                x => x.CreatedAt > minimumDate;

            var newBooks = await DbSet.GetWithCountAsync(pageHolder, expression);

            return newBooks;
        }
    }
}