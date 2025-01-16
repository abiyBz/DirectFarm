using DirectFarm.API.GetModel;
using DirectFarm.API.PostModel;
using DirectFarm.API.UtilModels;
using DirectFarm.Core.Contracts.Command;
using DirectFarm.Core.Contracts.Query;
using DirectFarm.Core.Entity;
using Infrastracture.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using Microsoft.Win32;

namespace DirectFarm.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarehouseController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<WarehouseController> _logger;
        private readonly IMediator mediator;
        public WarehouseController(ILogger<WarehouseController> logger, IMediator mediator, IConfiguration configuration)
        {
            _logger = logger;
            this.mediator = mediator;
            _configuration = configuration;
        }

        [HttpPost("RegisterFarmersProduct")]
        public async Task<Response<FarmerProductEntity>> RegisterFarmersProduct(FarmerProductEntity entity)
        {
            var result = await this.mediator.Send(new SaveFarmerProductCommand(entity));
            return result;
        }

        [HttpPost("RegisterWarehouse")]
        public async Task<Response<WarehouseEntity>> RegisterWarehouse(WarehouseEntity entity)
        {
            var result = await this.mediator.Send(new SaveWarehouseCommand(entity));
            return result;
        }

        [HttpPost("RegisterWarehouseManager")]
        public async Task<Response<WarehouseManagerEntity>> RegisterWarehouseManger(RegisterManagerModel model)
        {
            var response = new Response<WarehouseManagerEntity>();
            var registerResult = await RegisterUser(model);
            if (!registerResult.IsSuccessStatusCode)
            {
                response.Message = $"Registration failed: {registerResult}";
                return response;
            }
            response = await this.mediator.Send(new SaveManagerCommand(model.toMangager()));
            return response;
        }
        private async Task<HttpResponseMessage> RegisterUser(RegisterManagerModel register)
        {
            var registrationEndpoint = _configuration["Keycloak:UserRegistrationUrl"];
            string adminToken = await GetAdminAccessToken(); // Obtain admin token to create a user in Keycloak
            if (string.IsNullOrEmpty(registrationEndpoint) || string.IsNullOrEmpty(adminToken))
            {
                throw new Exception("Keycloak registration configuration is missing.");
            }

            var userPayload = new
            {
                username = register.Email,
                email = register.Email,
                firstName = register.FName,
                lastName = register.LName,
                enabled = true,
                credentials = new[]
                {
                    new
                    {
                        type = "password",
                        value = register.password,
                        temporary = false
                    }
                }
            };

            var jsonPayload = JsonSerializer.Serialize(userPayload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);

                // Register the user
                var registerResponse = await httpClient.PostAsync(registrationEndpoint, content);
                //if (!registerResponse.IsSuccessStatusCode)
                //{
                return registerResponse;
                //}

                //// Get the created user's ID
                //var getUserEndpoint = $"{registrationEndpoint}?username={register.Email}";
                //var getUserResponse = await httpClient.GetAsync(getUserEndpoint);
                //if (!getUserResponse.IsSuccessStatusCode)
                //{
                //    throw new Exception($"Failed to retrieve the user ID : {getUserResponse}");
                //}
                //var userData = await getUserResponse.Content.ReadAsStringAsync();
                //var users = JsonDocument.Parse(userData).RootElement.EnumerateArray();
                //var userId = users.First().GetProperty("id").GetString();

                //// Get the client UUID
                //var clientUuidEndpoint = $"{_configuration["Keycloak:BaseUrl"]}/admin/realms/directFarm/clients?clientId=realm-management";
                //var clientResponse = await httpClient.GetAsync(clientUuidEndpoint);
                //if (!clientResponse.IsSuccessStatusCode)
                //{
                //    throw new Exception($"Failed to retrieve client UUID: /n {clientResponse}");
                //}
                //var clientData = await clientResponse.Content.ReadAsStringAsync();
                //var clientUuid = JsonDocument.Parse(clientData).RootElement[0].GetProperty("id").GetString();

                //// Assign the role
                //var roleMappingEndpoint = $"http://localhost:8080/auth/admin/realms/directFarm/users/{userId}/role-mappings/clients/{clientUuid}";
                //var rolePayload = new[]
                //{
                //    new
                //    {
                //        id = "6816afa1-568b-410e-8460-106abb812d69", // Replace with your role ID
                //        name = "client" // Replace with your role name
                //    }
                //};
                //var roleJsonPayload = JsonSerializer.Serialize(rolePayload);
                //var roleContent = new StringContent(roleJsonPayload, Encoding.UTF8, "application/json");
                //var response = await httpClient.PostAsync(roleMappingEndpoint, roleContent);
                ////if (!response.IsSuccessStatusCode) 
                //{
                //    throw new Exception($"Issue while registering role : {response}");
                //}
                //return registerResponse;
            }
        }


        private async Task<string> GetAdminAccessToken()
        {
            var tokenEndpoint = _configuration["Keycloak:AdminTokenUrl"];
            string adminCid = _configuration["Keycloak:AdminClientId"];
            string adminCsecret = _configuration["Keycloak:AdminClientSecret"];
            string adminUsername = _configuration["Keycloak:AdminUsername"];
            string adminPassword = _configuration["Keycloak:AdminPassword"];

            if (string.IsNullOrEmpty(tokenEndpoint) || string.IsNullOrEmpty(adminCid) || string.IsNullOrEmpty(adminCsecret))
            {
                throw new Exception("Keycloak admin configuration is missing.");
            }

            var payload = new StringContent(
                $"grant_type=password&username={adminUsername}&password={adminPassword}&client_id={adminCid}&client_secret={adminCsecret}",
                Encoding.UTF8,
                "application/x-www-form-urlencoded");

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync(tokenEndpoint, payload);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = JsonSerializer.Deserialize<TokenModel>(responseContent);
                    return tokenResponse?.AccessToken ?? throw new Exception("Failed to retrieve admin token.");
                }
                else
                {
                    throw new Exception($"Admin token request failed: {responseContent}");
                }
            }
        }

        [HttpPost("GetFarmersProduct")]
        public async Task<Response<List<ProductEntity>>> GetFarmersProduct(BaseModel model)
        {
            var result = await this.mediator.Send(new GetFarmerProductsQuery(model.Id));
            return result;
        }
        [HttpPost("GetAllWarehouses")]
        public async Task<Response<List<WarehouseEntity>>> GetAllWarehouses()
        {
            var result = await this.mediator.Send(new GetAllWarehousesQuery());
            return result;
        }
    }
}
