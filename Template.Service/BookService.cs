using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Template.Common.Structs;
using Template.Data;
using Template.Entities.Concrete;
using Template.Service.Helpers;

namespace Template.Service
{
    public interface IBookService : IServiceBase<Book>
    {
        List<Book> GetNewBooks(int day, PageHolder pager);
    }

    public class BookService : ServiceBase<Book>, IBookService
    {
        public BookService(TemplateContext context) : base(context)
        {
        }
        
        public List<Book> GetNewBooks(int day, PageHolder pageHolder)
        {
            var minimumDate = DateTime.Now.Subtract(new TimeSpan(day, 0, 0));

            Expression<Func<Book, bool>> expression =
                x => x.CreatedAt > minimumDate;

            var newBooksQuery = DbSet.GetWithCount(pageHolder, expression);
            var newBooks = newBooksQuery.ToList();

            return newBooks;
        }
    }
}