using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace fcu_ucan.Models.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "{0}是必填的")]
        [MaxLength(256, ErrorMessage = "{0}最多{1}位")]
        [RegularExpression(@"^[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789]*$", ErrorMessage = "{0}只能是英文或數字")]
        [DataType(DataType.Text)]
        [JsonPropertyName("UserName")]
        [Display(Name = "使用者名稱")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "{0}是必填的")]
        [MinLength(8, ErrorMessage = "{0}最少{1}位")]
        [DataType(DataType.Password)]
        [JsonPropertyName("Password")]
        [Display(Name = "密碼")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "{0}是必填的")]
        [MinLength(8, ErrorMessage = "{0}最少{1}位")]
        [Compare("Password", ErrorMessage ="確認密碼與密碼不相符")]
        [DataType(DataType.Password)]
        [JsonPropertyName("ConfirmPassword")]
        [Display(Name = "確認密碼")]
        public string ConfirmPassword { get; set; }
    }
}