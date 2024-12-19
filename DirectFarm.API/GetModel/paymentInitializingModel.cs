using DirectFarm.Core.Entity;

namespace DirectFarm.API.GetModel
{
    public class paymentInitializingModel
    {
        public string Currency { get; set; }
        public OrderEntity Order { get; set; }
    }
}
