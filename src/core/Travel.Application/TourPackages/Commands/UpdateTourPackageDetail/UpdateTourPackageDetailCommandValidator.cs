using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Travel.Application.Common.Interfaces;

namespace Travel.Application.TourPackages.Commands.UpdateTourPackageDetail;

public class UpdateTourPackageDetailCommandValidator : AbstractValidator<UpdateTourPackageDetailCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTourPackageDetailCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.ListId).NotEmpty().WithMessage("ListId is required");
        RuleFor(x => x.WhatToExpect).NotEmpty().WithMessage("WhatToExpect is required");
        RuleFor(x => x.MapLocation).NotEmpty().WithMessage("MapLocation is required");
        RuleFor(x => x.Price).NotEmpty().WithMessage("Price is required");
        RuleFor(x => x.Duration).NotEmpty().WithMessage("Duration is required");
        RuleFor(x => x.InstantConfirmation).NotEmpty().WithMessage("InstantConfirmation is required");
        RuleFor(x => x.Currency).NotEmpty().WithMessage("Currency is required");
    }

    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.TourPackages.AllAsync(x => x.Name != name, cancellationToken: cancellationToken);
    }
}