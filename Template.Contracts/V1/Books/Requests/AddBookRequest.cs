using System;
using System.Collections.Generic;
using FluentValidation;

namespace Template.Contracts.V1.Books.Requests
{
    public class AddBookRequest
    {
        public DateTime? PublishDate { get; set; }
        public Guid? AuthorId { get; set; }
        public List<BookLanguageRequest> Languages { get; set; }
    }

    public class AddBookRequestValidator : AbstractValidator<AddBookRequest>
    {
        public AddBookRequestValidator()
        {
            RuleFor(x => x.PublishDate)
                .NotEqual(default(DateTime?))
                .WithName(x => nameof(x.PublishDate))
                .NotNull();

            RuleFor(x => x.AuthorId)
                .NotEqual(default(Guid?))
                .WithName(x => nameof(x.AuthorId))
                .NotNull();
        }
    }
}