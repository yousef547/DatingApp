namespace DatingApp_API.Helpers
{
    public class PageinationHeader
    {

        public PageinationHeader(int currentPagemint ,int itemsParPage,int totalItems,int totalPages )
        {
            CurrentPage = currentPagemint;
            ItemsPage = itemsParPage;
            TotalItems = totalItems;
            TotalPages = totalPages;
        }
        public int CurrentPage { get; set; }
        public int ItemsPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
