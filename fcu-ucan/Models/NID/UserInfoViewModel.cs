using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace fcu_ucan.Models.NID
{
    /// <summary>
    /// NID 使用者資訊
    /// </summary>
    public class UserInfoViewModel
    {
        [BindProperty(Name = "status")]
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [BindProperty(Name = "message")]
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [BindProperty(Name = "stu_id")]
        [JsonPropertyName("stu_id")]
        public string StuId { get; set; }
    }
}