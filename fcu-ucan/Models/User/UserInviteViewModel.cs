using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace fcu_ucan.Models.User
{
    public class UserInviteViewModel
    {
        [Required(ErrorMessage = "{0}是必填的")]
        [MaxLength(256, ErrorMessage = "{0}最多{1}位")]
        [EmailAddress(ErrorMessage = "{0}格式錯誤")]
        [DataType(DataType.EmailAddress)]
        [JsonPropertyName("Email")]
        [Display(Name = "電子郵件")]
        public string Email { get; set; }
    }
}