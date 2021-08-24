using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace fcu_ucan.Models.NID
{
    /// <summary>
    /// NID 登入回應資訊
    /// </summary>
    public class LoginRespondViewModel
    {
        [BindProperty(Name = "UserInfo")]
        [JsonPropertyName("UserInfo")]
        public List<UserInfoViewModel> UserInfo { get; set; }
    }
}