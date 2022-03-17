using Application.DTOs;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Clients.Queries.GetAllClients;

public class GetAllClientsQuery : IRequest<PagedResponse<List<ClientDto>>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }

    public class GetAllClientsQueryHandler : IRequestHandler<GetAllClientsQuery, PagedResponse<List<ClientDto>>>
    {
        private readonly IRepositoryAsync<Client> _repositoryAsync;
        private readonly IMapper _mapper;

        public GetAllClientsQueryHandler(IRepositoryAsync<Client> repositoryAsync, IMapper mapper)
        {
            _repositoryAsync = repositoryAsync;
            _mapper = mapper;
        }

        public async Task<PagedResponse<List<ClientDto>>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
        {
            List<Client> clients = await _repositoryAsync.ListAsync(new PagedClientsSpecification(request.PageNumber, request.PageSize, request.Name, request.Surname), cancellationToken);
            var clientsDto = _mapper.Map<List<ClientDto>>(clients);

            return new PagedResponse<List<ClientDto>>(clientsDto, request.PageNumber, request.PageSize);
        }
    }
}