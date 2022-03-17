using Application.Parameters;

namespace Application.Features.Clients.Queries.GetAllClients;

public class GetAllClientsParameters : RequestParameters
{
    public GetAllClientsParameters()
    {
        Name = string.Empty;
        Surname = string.Empty;
    }

    public string Name { get; set; }
    public string Surname { get; set; }
    
}