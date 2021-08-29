using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace fcu_ucan.Models.User
{
    public class UserEditViewModel
    {
        [JsonPropertyName("PhoneNumber")]
        [Display(Name = "手機號碼")]
        public string PhoneNumber { get; set; }
        
        [JsonPropertyName("PhoneNumberConfirmed")]
        [Display(Name = "手機號碼驗證")]
        public bool PhoneNumberConfirmed { get; set; }
        
        [JsonPropertyName("TwoFactorEnabled")]
        [Display(Name = "雙因素驗證")]
        public bool TwoFactorEnabled { get; set; }
        
        [JsonPropertyName("LockoutEnd")]
        [Display(Name = "使用者鎖定結束時間")]
        public DateTimeOffset? LockoutEnd { get; set; }
        
        [JsonPropertyName("LockoutEnabled")]
        [Display(Name = "可以鎖定使用者")]
        public bool LockoutEnabled { get; set; }

        [JsonPropertyName("AccessFailedCount")]
        [Display(Name = "失敗登入嘗試次數")]
        public int AccessFailedCount { get; set; }

        [JsonPropertyName("IsEnable")]
        [Display(Name = "啟用帳戶")]
        public bool IsEnable { get; set; }
    }
}