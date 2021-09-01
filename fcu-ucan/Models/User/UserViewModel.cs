using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace fcu_ucan.Models.User
{
    public class UserViewModel
    {
        [JsonPropertyName("Id")]
        [Display(Name = "使用者識別碼")]
        public string Id { get; set; }
        
        [JsonPropertyName("UserName")]
        [Display(Name = "使用者名稱")]
        public string UserName { get; set; }
        
        [JsonPropertyName("Email")]
        [Display(Name = "電子郵件")]
        public string Email { get; set; }
        
        [JsonPropertyName("EmailConfirmed")]
        [Display(Name = "電子郵件驗證")]
        public bool EmailConfirmed { get; set; }
        
        [JsonPropertyName("PhoneNumber")]
        [Display(Name = "手機號碼")]
        public string PhoneNumber { get; set; }
        
        [JsonPropertyName("PhoneNumberConfirmed")]
        [Display(Name = "手機號碼驗證")]
        public bool PhoneNumberConfirmed { get; set; }

        [JsonPropertyName("IsEnable")]
        [Display(Name = "啟用帳戶")]
        public bool IsEnable { get; set; }
        
        [JsonPropertyName("IsRecorder")]
        [Display(Name = "管理日誌權限")]
        public bool IsRecorder { get; set; }
        
        [JsonPropertyName("IsMember")]
        [Display(Name = "管理成員權限")]
        public bool IsMember { get; set; }
        
        [JsonPropertyName("IsUser")]
        [Display(Name = "管理使用者權限")]
        public bool IsUser { get; set; }
        
        [JsonPropertyName("IsUCAN")]
        [Display(Name = "UCAN 登入權限")]
        public bool IsUCAN { get; set; }
    }
}