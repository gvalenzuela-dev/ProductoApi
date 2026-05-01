using System;
using FluentValidation;
using ProductoApi.Application.Features.Products.Requests;

namespace ProductoApi.Application.Features.Products.Validators;

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{   
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Must(id => id != Guid.Empty)
            .WithMessage("Id must be a valid GUID.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(x => x.Price)
            .GreaterThan(0);
    }

}
