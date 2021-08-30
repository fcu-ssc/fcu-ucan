using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace fcu_ucan.Entities
{
    /// <summary>
    /// 成員
    /// </summary>
    [Index(nameof(NetworkId), Name = "NetworkIdIndex", IsUnique = true)]
    [Index(nameof(StudentId), Name = "StudentIdIndex", IsUnique = true)]
    public class Member
    {
        /// <summary>
        /// 成員識別碼
        /// </summary>
        [Key]
        [MaxLength(36)]
        public string Id { get; set; } = new SequentialGuidValueGenerator().Next(null!).ToString();

        /// <summary>
        /// NID 帳號
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string NetworkId { get; set; }
        
        /// <summary>
        /// UCAN 帳號
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string StudentId { get; set; }
    }
}