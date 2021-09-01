using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace fcu_ucan.Models.Manage
{
    public class UCANLoginViewModel
    {
        [Required(ErrorMessage = "{0}是必填的")]
        [DataType(DataType.Text)]
        [JsonPropertyName("UserName")]
        [Display(Name = "使用者名稱")]
        public string UserName { get; set; }
    }
}