namespace Application.Wrappers;

public class PagedResponse<T> : Response<T>
{
    public int PageNumber { get; }
    public int PageSize { get; }

    public PagedResponse(T data, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Data = data;
        Message = null;
        Succeeded = true;
        Errors = null;
    }
}