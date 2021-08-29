using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace fcu_ucan.Models.Member
{
    public class MemberViewModel
    {
        [JsonPropertyName("Id")]
        [Display(Name = "成員識別碼")]
        public string Id { get; set; }

        [JsonPropertyName("NetworkId")]
        [Display(Name = "NID 帳號")]
        public string NetworkId { get; set; }
        
        [JsonPropertyName("StudentId")]
        [Display(Name = "UCAN 帳號")]
        public string StudentId { get; set; }
    }
}