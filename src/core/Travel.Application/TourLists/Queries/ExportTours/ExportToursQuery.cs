using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Travel.Application.Common.Interfaces;

namespace Travel.Application.TourLists.Queries.ExportTours;

public class ExportToursQuery:IRequest<ExportToursVm>
{
    public int ListId { get; set; }
}

public class ExportTourQueryHandler : IRequestHandler<ExportToursQuery, ExportToursVm>
{
    public ExportTourQueryHandler(IApplicationDbContext context, IMapper mapper, ICsvFileBuilder fileBuilder)
    {
        _context = context;
        _mapper = mapper;
        _fileBuilder = fileBuilder;
    }

    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICsvFileBuilder _fileBuilder;
        
    public async Task<ExportToursVm> Handle(ExportToursQuery request, CancellationToken cancellationToken)
    {
        var vm = new ExportToursVm();

        var records = await _context.TourPackages
            .Where(t => t.ListId == request.ListId)
            .ProjectTo<TourPackageRecord>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        vm.Content = _fileBuilder.BuildTourPackagesFile(records);
        vm.ContentType = "text/csv";
        vm.FileName = "TourPackages.csv";

        return await Task.FromResult(vm);
    }
}