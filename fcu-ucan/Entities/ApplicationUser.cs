using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace fcu_ucan.Entities
{
    /// <summary>
    /// 使用者
    /// </summary>
    [Table("AspNetUsers")]
    [Index(nameof(NormalizedUserName), Name = "UserNameIndex", IsUnique = true)]
    [Index(nameof(NormalizedEmail), Name = "EmailIndex", IsUnique = true)]
    [Index(nameof(PhoneNumber), Name = "PhoneNumberIndex", IsUnique = true)]
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// 使用者識別碼
        /// </summary>
        [Key]
        [MaxLength(36)]
        [PersonalData]
        public override string Id { get; set; } = new SequentialGuidValueGenerator().Next(null!).ToString();

        /// <summary>
        /// 使用者名稱
        /// </summary>
        [MaxLength(256)]
        [ProtectedPersonalData]
        public override string UserName { get; set; }

        /// <summary>
        /// 標準化使用者名稱
        /// </summary>
        [MaxLength(256)]
        public override string NormalizedUserName { get; set; }

        /// <summary>
        /// 電子郵件
        /// </summary>
        [Required]
        [MaxLength(256)]
        [ProtectedPersonalData]
        public override string Email { get; set; }

        /// <summary>
        /// 標準化電子郵件
        /// </summary>
        [Required]
        [MaxLength(256)]
        public override string NormalizedEmail { get; set; }

        /// <summary>
        /// 電子郵件驗證
        /// </summary>
        [Required]
        [PersonalData]
        public override bool EmailConfirmed { get; set; } = false;

        /// <summary>
        /// 密碼雜湊
        /// </summary>
        public override string PasswordHash { get; set; }
        
        /// <summary>
        /// 安全標記
        /// </summary>
        public override string SecurityStamp { get; set; }
        
        /// <summary>
        /// 隨機標記
        /// </summary>
        [ConcurrencyCheck]
        public override string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 手機號碼
        /// </summary>
        [MaxLength(30)]
        [ProtectedPersonalData]
        public override string PhoneNumber { get; set; }

        /// <summary>
        /// 手機號碼驗證
        /// </summary>
        [Required]
        [PersonalData]
        public override bool PhoneNumberConfirmed { get; set; } = false;

        /// <summary>
        /// 雙因素驗證
        /// </summary>
        [Required]
        [PersonalData]
        public override bool TwoFactorEnabled { get; set; } = false;
        
        /// <summary>
        /// 使用者鎖定結束時間
        /// </summary>
        public override DateTimeOffset? LockoutEnd { get; set; }

        /// <summary>
        /// 可以鎖定使用者
        /// </summary>
        public override bool LockoutEnabled { get; set; }

        /// <summary>
        /// 失敗登入嘗試次數
        /// </summary>
        public override int AccessFailedCount { get; set; } = 0;
        
        /// <summary>
        /// 使用者聲明
        /// </summary>
        public ICollection<ApplicationUserClaim> Claims { get; set; }
        
        /// <summary>
        /// 使用者登入提供者
        /// </summary>
        public ICollection<ApplicationUserLogin> Logins { get; set; }
        
        /// <summary>
        /// 使用者驗證權杖
        /// </summary>
        public ICollection<ApplicationUserToken> Tokens { get; set; }

        /// <summary>
        /// 使用者與角色之間的連結
        /// </summary>
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
        
        /// <summary>
        /// 啟用帳戶
        /// </summary>
        [Required]
        [PersonalData]
        public bool IsEnable { get; set; } = false;
    }
}