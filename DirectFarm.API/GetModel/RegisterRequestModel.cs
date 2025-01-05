using DirectFarm.Core.Entity;

namespace DirectFarm.API.GetModel
{
    public class RegisterRequestModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        private string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Password { get; set; }

        public CustomerEntity toCustomer() 
        {
            Name = FirstName+"  "+LastName;
            var customer = new CustomerEntity(Guid.Empty, Name, Email, Phone, Address);
            return customer;
        }
    }
}
