using System.Text.Json.Serialization;

namespace DirectFarm.API.GetModel
{
    public class PaymentVerifyModel
    {
        [JsonPropertyName("event")]
        public string Event { get; set; }
        [JsonPropertyName("first_name")]
        public string first_name { get; set; } // Maps to "first_name"
        [JsonPropertyName("last_name")]
        public string last_name { get; set; } // Maps to "last_name"
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("mobile")]
        public string Mobile { get; set; }
        [JsonPropertyName("currency")]
        public string Currency { get; set; }
        [JsonPropertyName("amount")]
        public string Amount { get; set; }
        [JsonPropertyName("charge")]
        public string Charge { get; set; }
        [JsonPropertyName("status")] 
        public string Status { get; set; }
        [JsonPropertyName("failure_reason")]
        public string failure_reason { get; set; } // Maps to "failure_reason"
        [JsonPropertyName("mode")] 
        public string Mode { get; set; }
        [JsonPropertyName("reference")]
        public string Reference { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime created_at { get; set; } // Maps to "created_at"
        [JsonPropertyName("updated_at")]
        public DateTime updated_at { get; set; } // Maps to "updated_at"
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("tx_ref")]
        public string tx_ref { get; set; } // Maps to "tx_ref"
        [JsonPropertyName("payment_method")]
        public string payment_method { get; set; } // Maps to "payment_method"
        [JsonPropertyName("customization")]
        public Customization Customization { get; set; }
        [JsonPropertyName("meta")]
        public object Meta { get; set; }
    }

    public class Customization
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("logo")]
        public string Logo { get; set; }
    }

}
