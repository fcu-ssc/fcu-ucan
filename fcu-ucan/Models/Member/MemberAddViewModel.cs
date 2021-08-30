using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace fcu_ucan.Models.Member
{
    public class MemberAddViewModel
    {
        [Required(ErrorMessage = "{0}是必填的")]
        [MaxLength(50, ErrorMessage = "{0}最多{1}位")]
        [DataType(DataType.Text)]
        [JsonPropertyName("NetworkId")]
        [Display(Name = "NID 帳號")]
        public string NetworkId { get; set; }
        
        [Required(ErrorMessage = "{0}是必填的")]
        [MaxLength(50, ErrorMessage = "{0}最多{1}位")]
        [DataType(DataType.Text)]
        [JsonPropertyName("StudentId")]
        [Display(Name = "UCAN 帳號")]
        public string StudentId { get; set; }
    }
}