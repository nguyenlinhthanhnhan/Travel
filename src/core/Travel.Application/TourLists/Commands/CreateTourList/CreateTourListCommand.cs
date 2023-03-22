using MediatR;

namespace Travel.Application.TourLists.Commands.CreateTourList;

public class CreateTourListCommand : IRequest<int>
{
    public string City { get; set; }
    public string Country { get; set; }
    public string About { get; set; }
}