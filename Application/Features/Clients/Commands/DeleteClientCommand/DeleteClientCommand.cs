using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Features.Clients.Commands.DeleteClientCommand;

public class DeleteClientCommand: IRequest<Response<int>>
{
    public int Id { get; set; }
}

public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, Response<int>>
{
    private readonly IRepositoryAsync<Client> _repositoryAsync;

    public DeleteClientCommandHandler(IRepositoryAsync<Client> repositoryAsync)
    {
        _repositoryAsync = repositoryAsync;
    }

    public async Task<Response<int>> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        Client? client = await _repositoryAsync.GetByIdAsync(request.Id, cancellationToken);
        if (client == null)
        {
            throw new KeyNotFoundException($"Registro no encontrado con el id {request.Id}");
        }

        await _repositoryAsync.DeleteAsync(client, cancellationToken);

        return new Response<int>(client.Id);
    }
}