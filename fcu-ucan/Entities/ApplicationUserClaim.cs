using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace fcu_ucan.Entities
{
    /// <summary>
    /// 使用者聲明
    /// </summary>
    [Table("AspNetUserClaims")]
    public class ApplicationUserClaim : IdentityUserClaim<string>
    {
        /// <summary>
        /// 使用者聲明識別碼
        /// </summary>
        [Key]
        public override int Id { get; set; }

        /// <summary>
        /// 使用者聲明類型
        /// </summary>
        public override string ClaimType { get; set; }

        /// <summary>
        /// 使用者聲明值
        /// </summary>
        public override string ClaimValue { get; set; }
        
        /// <summary>
        /// 使用者識別碼
        /// </summary>
        [Required]
        [MaxLength(36)]
        public override string UserId { get; set; }

        /// <summary>
        /// 使用者
        /// </summary>
        public ApplicationUser User { get; set; }
    }
}