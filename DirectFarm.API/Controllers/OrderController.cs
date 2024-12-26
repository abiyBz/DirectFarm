using DirectFarm.API.GetModel;
using DirectFarm.Core.Contracts.Query;
using DirectFarm.Core.Contracts.Command;
using DirectFarm.Core.Entity;
using Infrastracture.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace DirectFarm.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OrderController> _logger;
        private readonly IMediator mediator;
        public OrderController(ILogger<OrderController> logger, HttpClient httpClient, IMediator mediator)
        {
            _logger = logger;
            this.mediator = mediator;
            _httpClient = httpClient;
        }
        [HttpPost("IntializePayment")]//, Authorize]
        public async Task<Response<String>> PaymentInitialize(Guid orderId)
        {
            var order = await this.mediator.Send(new GetOrderQuery(orderId));
            var response = new Response<String>();
            if (order.Data == null) 
            {
                response.Message = order.Message;
                return response;
            }
            var unique = orderId;
            var TotalAmount = order.Data.TotalAmount;

            //foreach (var pOrders in order.ProductOrders)
            //{
            //    pOrders.Amount = pOrders.PriceAtPurchase + pOrders.Quantity;
            //    order.TotalAmount += pOrders.Amount;
            //}

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.chapa.co/v1/transaction/initialize"),
                Content = new StringContent(JsonSerializer.Serialize(new
                {
                    amount = TotalAmount.ToString(),
                    currency = "ETB",
                    tx_ref = unique.ToString(),
                    //return_url = r_url,
                    //callback_url = "https://brave-clocks-jam.loca.lt/api/Order/verify",
                }), System.Text.Encoding.UTF8, "application/json")
            };


            request.Headers.Add("Authorization", "Bearer CHASECK_TEST-mrwVfokQVSm1FUUfx1xFwsvbRsHW6BsX");

            var responseChapa = await _httpClient.SendAsync(request);

            var responseContent = await responseChapa.Content.ReadAsStringAsync();

            if (responseChapa.IsSuccessStatusCode)
            {
                var responseObject = JsonDocument.Parse(responseContent);
                var checkoutUrl = responseObject.RootElement.GetProperty("data").GetProperty("checkout_url").GetString();
                response.Data = checkoutUrl;
                response.Message = "Payment request granted!";
                return response;
            }
            else
            {
                response.Message = StatusCode((int)responseChapa.StatusCode, responseContent).ToString();
                return response;
            }
        }
        [HttpPost("VerifyPayment")]
        public async Task<Response<bool>> verifyAndOrder(Guid orderId)
        {
            var response = new Response<bool>();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://api.chapa.co/v1/transaction/verify/" + orderId.ToString())
            };
            request.Headers.Add("Authorization", "Bearer CHASECK_TEST-mrwVfokQVSm1FUUfx1xFwsvbRsHW6BsX");

            var responseChapa = await _httpClient.SendAsync(request);
            response.Message = await responseChapa.Content.ReadAsStringAsync();
            response.Data = responseChapa.IsSuccessStatusCode;

            if (!responseChapa.IsSuccessStatusCode)
            {
                Console.WriteLine("Failed to initialize transaction with Chapa. Status Code: {StatusCode}, Response: {Response}");
                await this.mediator.Send(new RecordPaymentCommand(orderId, false));
            }
            else 
            {
                response = await this.mediator.Send(new RecordPaymentCommand(orderId, true));
            }
            return response;
        }

        [HttpPost("PlaceOrder")]
        public async Task<Response<OrderEntity>> PlaceOrder(SaveOrderModel order)
        {
            return await this.mediator.Send(new PlaceOrderCommand(order.toOrderEntity()));
        }
    }
}
