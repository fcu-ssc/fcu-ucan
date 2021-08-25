using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace fcu_ucan.Models.NID
{
    /// <summary>
    /// NID 回應資訊
    /// </summary>
    public class RespondViewModel
    {
        [BindProperty(Name = "status")]
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [BindProperty(Name = "message")]
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [BindProperty(Name = "user_code")]
        [JsonPropertyName("user_code")]
        public string UserCode { get; set; }
    }
}