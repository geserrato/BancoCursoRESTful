using Application.Wrappers;
using MediatR;

namespace Application.Features.Clients.Commands.CreateClientCommand;

public abstract class CreateClientCommand : IRequest<Response<int>>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
}

public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Response<int>>
{
    public async Task<Response<int>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}