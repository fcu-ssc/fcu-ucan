using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace fcu_ucan.Entities
{
    /// <summary>
    /// 使用者登入提供者
    /// </summary>
    [Table("AspNetUserLogins")]
    public class ApplicationUserLogin : IdentityUserLogin<string>
    {
        /// <summary>
        /// 登入提供者
        /// </summary>
        public override string LoginProvider { get; set; }

        /// <summary>
        /// 提供者識別碼
        /// </summary>
        public override string ProviderKey { get; set; }

        /// <summary>
        /// 易記名稱
        /// </summary>
        public override string ProviderDisplayName { get; set; }
        
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