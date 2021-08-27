using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace fcu_ucan.Entities
{
    /// <summary>
    /// 使用者與角色之間的連結
    /// </summary>
    [Table("AspNetUserRoles")]
    public class ApplicationUserRole : IdentityUserRole<string>
    {
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