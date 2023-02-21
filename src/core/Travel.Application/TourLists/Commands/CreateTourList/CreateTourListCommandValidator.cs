using FluentValidation;
using Travel.Application.Common.Interfaces;

namespace Travel.Application.TourLists.Commands.CreateTourList;

public class CreateTourListCommandValidator:AbstractValidator<CreateTourListCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateTourListCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.City).NotEmpty().WithMessage("City is required").MaximumLength(200)
            .WithMessage("City must not exceed 90 characters.");
        
        RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required").MaximumLength(200)
            .WithMessage("Country must not exceed 60 characters.");

        RuleFor(x => x.About).NotEmpty().WithMessage("About is required");
    }
}