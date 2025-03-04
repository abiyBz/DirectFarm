﻿using DirectFarm.API.GetModel;
using DirectFarm.API.PostModel;
using DirectFarm.API.UtilModels;
using DirectFarm.Core.Contracts.Command;
using DirectFarm.Core.Entity;
using Infrastracture.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DirectFarm.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly ILogger<AdminController> _logger;
        private readonly IMediator mediator;
        public AdminController(ILogger<AdminController> logger, IMediator mediator, IConfiguration configuration)
        {
            _logger = logger;
            this.mediator = mediator;
            _configuration = configuration;
        }
        [HttpPost("AdminLogin")]
        public async Task<Response<TokenResponseModel<bool>>> Login(LoginRequestModel login)
        {
            var response = new Response<TokenResponseModel<bool>>();
            response.Data = new TokenResponseModel<bool>();
            try
            {
                var model = await IsValidUser(login.Email, login.Password);
                if (model != null) // Example validation
                {
                    response.Data = new TokenResponseModel<bool>(model, true);
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.ReadToken(model.AccessToken) as JwtSecurityToken;

                    if (securityToken != null)
                    {
                        // Fetch the resource_access claim
                        var resourceAccessClaim = securityToken.Claims.FirstOrDefault(c => c.Type == "resource_access")?.Value;
                        if (!string.IsNullOrEmpty(resourceAccessClaim))
                        {
                            var resourceAccess = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, List<string>>>>(resourceAccessClaim);
                            if (resourceAccess != null && resourceAccess.TryGetValue("directClient", out var clientRoles))
                            {
                                // Extract roles from directClient
                                var roles = clientRoles["roles"];
                                foreach (var role in roles)
                                {
                                    if (role == "admin")
                                    {
                                        response.Message = "Admin Login successful!";
                                        return response;
                                    }
                                }
                            }
                        }
                    }
                    throw new Exception("User is not an admin!");
                }
                else
                {
                    throw new Exception("Admin Login failed!");
                }
            }
            catch (Exception ex)
            {
                response.Data = null;
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
    }
}
