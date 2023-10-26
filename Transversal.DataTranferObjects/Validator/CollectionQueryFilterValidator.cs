using FluentValidation;
using System;
using Transversal.QueryFilters;

namespace Transversal.Validator
{
    public class CollectionQueryFilterValidator : AbstractValidator<CollectionQueryFilter>
    {
        public CollectionQueryFilterValidator()
        { 
            RuleFor(c => c.Station).MaximumLength(20).WithMessage("Station must not exceed 20 characters.");
        }
    }
}
