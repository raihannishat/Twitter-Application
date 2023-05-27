namespace WebApi.Extensions;

public static class HttpExtensions
{
    public static void AddPaginationHeader(this HttpResponse response, int currentpage,
        int itemsPerpage, int totalItems, int totalPages)
    {
        var paginationHeader = new
        {
            currentpage,
            itemsPerpage,
            totalItems,
            totalPages
        };

        response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader));
    }
}
