using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace fcu_ucan.Models.User;

public class UserViewModel
{
    [JsonPropertyName("Id")]
    [Display(Name = "使用者識別碼")]
    public string Id { get; set; } = default!;
        
    [JsonPropertyName("UserName")]
    [Display(Name = "使用者名稱")]
    public string UserName { get; set; } = default!;
        
    [JsonPropertyName("Email")]
    [Display(Name = "電子郵件")]
    public string Email { get; set; } = default!;
        
    [JsonPropertyName("EmailConfirmed")]
    [Display(Name = "電子郵件驗證")]
    public bool EmailConfirmed { get; set; }
}