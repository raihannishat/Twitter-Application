namespace Application.Common.Models.Response;

public class PagedResponse<T>
{
    public PagedResponse()
    {
    }

    public PagedResponse(IEnumerable<T> data, int pageNumber, int pageSize, int totalCount)
    {
        Data = data;
        CurrentPage = pageNumber;
        PageSize = pageSize;
        TotalPage = (int)Math.Ceiling((totalCount / (double)pageSize));
        TotalCount = totalCount;
    }

    public IEnumerable<T> Data { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPage { get; set; }
    public int TotalCount { get; set; }

    public static PagedResponse<T> CreateAsync(IEnumerable<T> data, int pageNumber, int pageSize, int count)
    {
        return new PagedResponse<T>(data, pageNumber, pageSize, count);
    }
}