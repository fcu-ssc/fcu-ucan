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
        
        [Required(ErrorMessage = "{0}是必填的")]
        [JsonPropertyName("IsRecorder")]
        [Display(Name = "管理日誌權限")]
        public bool IsRecorder { get; set; }
        
        [Required(ErrorMessage = "{0}是必填的")]
        [JsonPropertyName("IsMember")]
        [Display(Name = "管理成員權限")]
        public bool IsMember { get; set; }
        
        [Required(ErrorMessage = "{0}是必填的")]
        [JsonPropertyName("IsUser")]
        [Display(Name = "管理使用者權限")]
        public bool IsUser { get; set; }
        
        [Required(ErrorMessage = "{0}是必填的")]
        [JsonPropertyName("IsUCAN")]
        [Display(Name = "UCAN 登入權限")]
        public bool IsUCAN { get; set; }
    }
}