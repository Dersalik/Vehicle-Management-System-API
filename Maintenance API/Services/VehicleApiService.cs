using Maintenance_API.DTO;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.Identity.Abstractions;



namespace Maintenance_API.Services
{
    public class VehicleApiService : IVehicleApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<VehicleApiService> _logger;

        public VehicleApiService(IHttpClientFactory httpClientFactory, ILogger<VehicleApiService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;

        }


        public async Task<VehicleDTO> GetVehicleById(int id)
        {
            try
            {


                var httpClient = _httpClientFactory.CreateClient("VehicleAPI");
               
                var response = await httpClient.GetAsync($"/api/v1.0/vehicles/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve vehicle with id {id}. Status code: {response.StatusCode}");
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var vehicle = JsonConvert.DeserializeObject<VehicleDTO>(content);
                vehicle!.StatusCode= (int)response.StatusCode;
                _logger.LogInformation($"Retrieved vehicle with id {id}");
                return vehicle;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving vehicle with id {id}");
                return null;
            }
        }
    }
}
