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
                var entity = await IsValidUser(login.Email, login.Password);
                if (entity != null) // Example validation
                {
                    entity.Email = login.Email;
                    //await this.mediator.Send(new SaveTokenCommand(entity));
                    response.Message = "Login successful!";
                    response.Data = new TokenResponseModel(entity);
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

        private async Task<CustomerEntity> IsValidUser(string email, string password)
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
                    var tokenResponse = JsonSerializer.Deserialize<CustomerEntity>(responseContent);
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
    }
}
