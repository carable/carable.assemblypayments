﻿using Newtonsoft.Json;

namespace Carable.AssemblyPayments.Entities
{
    public class CardToken
    {
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }
    }
}
