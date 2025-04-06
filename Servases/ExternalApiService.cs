using InventoryManagementWithExpirationDatesSystem.DTOs;
using Newtonsoft.Json;

namespace InventoryManagementWithExpirationDatesSystem.Services
{
    public interface IExternalApiService
    {
        Task<IEnumerable<ItemDTO>> GetExternalItemsAsync();  // Example: fetch data from external API
    }

    public class ExternalApiService : IExternalApiService
    {
        private readonly HttpClient _httpClient;

        public ExternalApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ItemDTO>> GetExternalItemsAsync()
        {
            var url = "https://jsonplaceholder.typicode.com/posts";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var items = JsonConvert.DeserializeObject<IEnumerable<ItemDTO>>(json);

                return items;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("HTTP error while calling the external API.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred while calling the external API.", ex);
            }
        }


    }
}
