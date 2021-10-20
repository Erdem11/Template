using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Template.Data;
using Template.Entities.Concrete;
using Template.Service.Abstract;
using Template.Service.Helpers;

namespace Template.Service.Concrete
{
    public class BookService : ServiceBase<Book>
    {
        public BookService(TemplateContext context) : base(context)
        {
        }

        public List<Book> GetNewBooks(int day)
        {
            var newBooksQuery = DbSet.Where(x => x.CreatedAt > DateTime.Now.Subtract(new TimeSpan(day, 0, 0)));
            var newBooks = newBooksQuery.ToList();

            DbSet.Include(x => x.Author).GetWithCount(default,default,out var _,default);
            DbSet.GetWithCount(default,default,out var _,default);
            this.GetWithCount(default, default, out var _, default);
            
            return newBooks;
        }
    }
}