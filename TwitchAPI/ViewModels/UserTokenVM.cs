﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TwitchAPI.ViewModels
{
    public partial class UserToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonPropertyName("scope")]
        public string[] Scope { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
    }
}
