﻿namespace DatingApp_API.Helpers
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
        public string CurrentUsername { get; set; }
        public string Gender { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 150;
        public string OrderBy { get; set; } = "lastActive";
    }
}
