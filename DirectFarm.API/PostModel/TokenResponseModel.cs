using DirectFarm.API.UtilModels;
using DirectFarm.Core.Entity;

namespace DirectFarm.API.PostModel
{
    public class TokenResponseModel
    {
        public string? AccessToken { get; set; }
        //[JsonPropertyName("refresh_token")]
        //public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        //[JsonPropertyName("refresh_expires_in")]
        //public int RefreshExpiresIn { get; set; }
        public CustomerEntity? Customer { get; set; }
        public TokenResponseModel() { }
        public TokenResponseModel(TokenModel tokenEntity) 
        {
            AccessToken = tokenEntity.AccessToken;
            ExpiresIn = tokenEntity.ExpiresIn;
        }

        public TokenResponseModel(TokenModel tokenEntity, CustomerEntity? customer)
        {
            AccessToken = tokenEntity.AccessToken;
            ExpiresIn = tokenEntity.ExpiresIn;
            Customer = customer;
        }
        //public TokenResponseModel(TokenEntity tokenEntity)
        //{
        //    AccessToken = tokenEntity.AccessToken;
        //    ExpiresIn = tokenEntity.ExpiresIn;
        //}
    }
}
