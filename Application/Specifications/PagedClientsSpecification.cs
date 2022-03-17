using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specifications;

public sealed class PagedClientsSpecification : Specification<Client>
{
    public PagedClientsSpecification(int pageNumber, int pageSize, string name, string surname)
    {
        Query.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        if (!string.IsNullOrEmpty(name))
        {
            Query.Search(p => p.Name, "%" + name + "%");
        }
        
        if (!string.IsNullOrEmpty(surname))
        {
            Query.Search(client => client.Surname, "%" + surname + "%");
        }
    }
}