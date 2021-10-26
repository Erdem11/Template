using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Template.Common.Structs;
using Template.Data;
using Template.Domain.Dto;
using Template.Service.Helpers;

namespace Template.Service
{
    public interface ITagService : IServiceBase<Tag>
    {
        Tag Add(List<TagLanguage> languages, Guid userId);
        List<Tag> GetAllTags(PaginationFilter filter);
    }
    
    public class TagService: ServiceBase<Tag>, ITagService
    {
        private readonly TemplateContext _context;
        public TagService(TemplateContext context) : base(context)
        {
            _context = context;
        }

        public Tag Add(List<TagLanguage> languages, Guid userId)
        {
            var tag = new Tag();
            tag.Languages = languages;
            tag.AddedUserId = userId;

            _context.Tags.Add(tag);
            _context.SaveChanges();

            return tag;
        }
        public List<Tag> GetAllTags(PaginationFilter filter)
        {
            var newBooksQuery = DbSet.GetPagedQuery(filter);
            var newBooks = newBooksQuery.ToList();

            return newBooks;
        }
    }
}