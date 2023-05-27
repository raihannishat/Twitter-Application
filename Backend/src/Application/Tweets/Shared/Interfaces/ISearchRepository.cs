namespace Application.Tweets.Shared.Interfaces;

public interface ISearchRepository : IRepository<Search>
{
    IEnumerable<Search> GetHashtagByFuzzySearch(string name);

    Task<IEnumerable<Search>> GetHashtagWithPagination(int pageNumber);
    Task<IEnumerable<Search>> GetHashtagByRegex(string name);

}
