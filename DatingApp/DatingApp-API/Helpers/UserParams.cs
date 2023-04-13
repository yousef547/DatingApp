namespace DatingApp_API.Helpers
{
    public class UserParams
    {
        private const int maxPageSize = 50;
        public int pageNumber { get; set; } = 1;
        private int _pageSize  = 14;
        public int pageSize
        {
            get => _pageSize;
            set => _pageSize = (value>maxPageSize)?maxPageSize : value;
        }
    }
}
