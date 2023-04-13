using DatingApp_API.Helpers;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace DatingApp_API.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, int currentPage, int itemsParPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PageinationHeader(currentPage, itemsParPage, totalItems, totalPages);
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader, options));
            response.Headers.Add("Access-Control-Expose-headers", "Pagination");
        }
    }
}
