using DirectFarm.API.GetModel;
using DirectFarm.API.PostModel;
using Infrastracture.Base;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using DirectFarm.Core.Entity;
using DirectFarm.API.UtilModels;
using DirectFarm.Core.Contracts.Command;
using Microsoft.AspNetCore.Authorization;
using DirectFarm.Core.Contracts.Query;
using Microsoft.Win32;

namespace DirectFarm.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CustomerController> _logger;
        private readonly IMediator mediator;

        public CustomerController(ILogger<CustomerController> logger, IMediator mediator, IConfiguration configuration)
        {
            _logger = logger;
            this.mediator = mediator;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<Response<TokenResponseModel>> Login(LoginRequestModel login)
        {
            var response = new Response<TokenResponseModel>();
            try
            {
                var model = await IsValidUser(login.Email, login.Password);
                if (model != null) // Example validation
                {
                    var entity = await this.mediator.Send(new SaveTokenCommand(login.Email, model.RefreshToken));
                    response.Data = new TokenResponseModel(model, entity.Data);
                    response.Message = "Login successful!";
                }
                else
                    response.Message = "Login failed!";
            }
            catch (Exception ex)
            {
                response.Ex = ex;
                response.Message = ex.Message;
            }
            return response;
        }

        private async Task<TokenModel> IsValidUser(string email, string password)
        {
            var tokenEndpoint = _configuration["Keycloak:AuthorizationUrltok"];
            string Cid = _configuration["Keycloak:client_id"];
            string Csecret = _configuration["Keycloak:client_secret"];
            if (string.IsNullOrEmpty(tokenEndpoint) || string.IsNullOrEmpty(Cid) || string.IsNullOrEmpty(Csecret))
            {
                // Handle missing configuration
                throw new Exception("Keycloak configuration is missing.");
            }


            // Prepare the request payload
            var payload = new StringContent(
                $"grant_type=password&username={email}&password={password}&client_id={Cid}&client_secret={Csecret}",
                Encoding.UTF8,
                "application/x-www-form-urlencoded");

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync(tokenEndpoint, payload);

                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = JsonSerializer.Deserialize<TokenModel>(responseContent);
                    return tokenResponse;
                    // Example of making an authorized API call using the access token
                }
                else
                {
                    // Log the error message if the login fails
                    throw new Exception($"Login failed: {responseContent}");
                }
            }

        }

        [HttpPost("Register")]
        public async Task<Response<TokenResponseModel>> RegisterAndLogin(RegisterRequestModel register)
        {
            var response = new Response<TokenResponseModel>();
            try
            {
                // Step 1: Register the user in Keycloak
                var registerResult = await RegisterUser(register);
                if (!registerResult.IsSuccessStatusCode)
                {
                    response.Message = $"Registration failed: {registerResult}";
                    return response;
                }

                // Step 2: Attempt login for the newly registered user

                var model = await IsValidUser(register.Email, register.Password);
                if (model != null) // Example validation
                {
                    var entity = await this.mediator.Send(new RegisterCustomerCommand(register.toCustomer(), model.RefreshToken));
                    response.Data = new TokenResponseModel(model, entity.Data);
                    response.Message = "Registration successful!";
                }
                else
                    response.Message = "Registration failed!";
            }
            catch (Exception ex)
            {
                response.Ex = ex;
                response.Message = ex.Message;
            }
            return response;
        }

        private async Task<HttpResponseMessage> RegisterUser(RegisterRequestModel register)
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
                firstName = register.FirstName,
                lastName = register.LastName,
                enabled = true,
                credentials = new[]
                {
            new
            {
                type = "password",
                value = register.Password,
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
                if (!registerResponse.IsSuccessStatusCode)
                {
                    return registerResponse;
                }

                // Get the created user's ID
                var getUserEndpoint = $"{registrationEndpoint}?username={register.Email}";
                var getUserResponse = await httpClient.GetAsync(getUserEndpoint);
                if (!getUserResponse.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to retrieve the user ID : {getUserResponse}");
                }
                var userData = await getUserResponse.Content.ReadAsStringAsync();
                var users = JsonDocument.Parse(userData).RootElement.EnumerateArray();
                var userId = users.First().GetProperty("id").GetString();

                // Get the client UUID
                var clientUuidEndpoint = $"{_configuration["Keycloak:BaseUrl"]}/admin/realms/directFarm/clients?clientId=realm-management";
                var clientResponse = await httpClient.GetAsync(clientUuidEndpoint);
                if (!clientResponse.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to retrieve client UUID: /n {clientResponse}");
                }
                var clientData = await clientResponse.Content.ReadAsStringAsync();
                var clientUuid = JsonDocument.Parse(clientData).RootElement[0].GetProperty("id").GetString();

                // Assign the role
                var roleMappingEndpoint = $"http://localhost:8080/auth/admin/realms/directFarm/users/{userId}/role-mappings/clients/{clientUuid}";
                var rolePayload = new[]
                {
                    new
                    {
                        id = "6816afa1-568b-410e-8460-106abb812d69", // Replace with your role ID
                        name = "client" // Replace with your role name
                    }
                };
                var roleJsonPayload = JsonSerializer.Serialize(rolePayload);
                var roleContent = new StringContent(roleJsonPayload, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(roleMappingEndpoint, roleContent);
                //if (!response.IsSuccessStatusCode) 
                //{
                //    throw new Exception($"Issue while registering role : {response}");
                //}
                return registerResponse;
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

        [HttpPost("Refresh")]
        public async Task<Response<TokenResponseModel>> Refresh(string email)
        {
            var response = new Response<TokenResponseModel>();
            try
            {
                var tokenEndpoint = _configuration["Keycloak:AuthorizationUrltok"];
                string clientId = _configuration["Keycloak:client_id"];
                string clientSecret = _configuration["Keycloak:client_secret"];

                if (string.IsNullOrEmpty(tokenEndpoint) || string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
                {
                    throw new Exception("Keycloak configuration is missing.");
                }
                var refreshToken = await this.mediator.Send(new GetRefreshTokenQuery(email));

                // Prepare the request payload
                var payload = new StringContent(
                    $"grant_type=refresh_token&refresh_token={refreshToken.Data}&client_id={clientId}&client_secret={clientSecret}",
                    Encoding.UTF8,
                    "application/x-www-form-urlencoded");

                using (var httpClient = new HttpClient())
                {
                    var refreshResponse = await httpClient.PostAsync(tokenEndpoint, payload);
                    var responseContent = await refreshResponse.Content.ReadAsStringAsync();

                    if (refreshResponse.IsSuccessStatusCode)
                    {
                        var tokenEntity = JsonSerializer.Deserialize<TokenModel>(responseContent);
                        response.Data = new TokenResponseModel(tokenEntity);
                        response.Message = "Token refreshed successfully!";
                    }
                    else
                    {
                        response.Message = "Failed to refresh token.";
                        response.Ex = new Exception($"Keycloak error: {responseContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Ex = ex;
            }

            return response;
        }

        [HttpPost("GetCustomerOrders")]
        public async Task<Response<List<OrderEntity>>> GetCustomerOrders(BaseModel model) 
        {
            var response = await this.mediator.Send(new GetCustomerOrdersQuery(model.Id));
            return response;
        }
        [HttpPost("ReviewProduct")]
        public async Task<Response<ReviewEntity>> ReviewProduct(ReviewEntity entity)
        {
            var response = await this.mediator.Send(new SubmitReviewCommand(entity));
            return response;
        }
    }
}
