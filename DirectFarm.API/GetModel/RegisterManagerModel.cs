using DirectFarm.Core.Entity;

namespace DirectFarm.API.GetModel
{
    public class RegisterManagerModel
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string FName { get; set; } = string.Empty;
        public string LName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Status { get; set; } = "active";
        public string password { get; set; } = string.Empty;
        public RegisterManagerModel() { }
        public WarehouseManagerEntity toMangager() 
        {
            return new WarehouseManagerEntity(Id, FName+" "+LName, Email, Phone, Status);
        }
    }
}
