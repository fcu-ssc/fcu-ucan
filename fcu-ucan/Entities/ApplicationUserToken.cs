using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace fcu_ucan.Entities
{
    /// <summary>
    /// 使用者驗證權杖
    /// </summary>
    [Table("AspNetUserTokens")]
    public class ApplicationUserToken : IdentityUserToken<string>
    {
        /// <summary>
        /// 使用者驗證權杖提供者
        /// </summary>
        public override string LoginProvider { get; set; }

        /// <summary>
        /// 使用者驗證權杖名稱
        /// </summary>
        public override string Name { get; set; }

        /// <summary>
        /// 使用者驗證權杖值
        /// </summary>
        [ProtectedPersonalData]
        public override string Value { get; set; }

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