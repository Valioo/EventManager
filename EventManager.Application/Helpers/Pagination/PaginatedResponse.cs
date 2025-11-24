namespace EventManager.Application.Helpers.Pagination;

public class PaginatedResponse<T> where T : class
{
    public IList<T> Result { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int MaximumPages { get; set; }
}
