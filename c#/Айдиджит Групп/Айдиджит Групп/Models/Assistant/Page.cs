namespace Айдиджит_Групп.Assistant;

public sealed class Page<TEntity> where TEntity : class
{
    public Page(IEnumerable<TEntity> items, int countPages, int page, int countItems)
    {
        Items = items.ToList();
        Pagination = new(countPages, page, countItems);
    }

    internal Page()
        => Pagination = new();

    public List<TEntity> Items { get; private set; } = new();

    public PaginationPage Pagination { get; private set; }

    public sealed class PaginationPage
    {
        public PaginationPage(int countPages, int page, int countItems)
        {
            CountPages = countPages;
            Page = page;
            CountItems = countItems;
        }

        internal PaginationPage() { }

        public int CountPages { get; private set; }
        public int Page { get; private set; }
        public int CountItems { get; private set; } 
    }
}