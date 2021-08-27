using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace fcu_ucan.Entities
{
    /// <summary>
    /// 角色聲明
    /// </summary>
    [Table("AspNetRoleClaims")]
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        /// <summary>
        /// 角色聲明識別碼
        /// </summary>
        [Key]
        public override int Id { get; set; }

        /// <summary>
        /// 角色聲明類型
        /// </summary>
        public override string ClaimType { get; set; }

        /// <summary>
        /// 角色聲明值
        /// </summary>
        public override string ClaimValue { get; set; }

        /// <summary>
        /// 角色識別碼
        /// </summary>
        [Required]
        [MaxLength(36)]
        public override string RoleId { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public ApplicationRole Role { get; set; }
    }
}