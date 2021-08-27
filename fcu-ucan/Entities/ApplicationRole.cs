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
    /// 角色
    /// </summary>
    [Table("AspNetRoles")]
    [Index(nameof(NormalizedName), Name = "RoleNameIndex", IsUnique = true)]
    public class ApplicationRole : IdentityRole<string>
    {
        /// <summary>
        /// 角色識別碼
        /// </summary>
        [Key]
        [MaxLength(36)]
        public override string Id { get; set; } = new SequentialGuidValueGenerator().Next(null!).ToString();

        /// <summary>
        /// 角色名稱
        /// </summary>
        [Required]
        [MaxLength(256)]
        public override string Name { get; set; }

        /// <summary>
        /// 角色標準化名稱
        /// </summary>
        [Required]
        [MaxLength(256)]
        public override string NormalizedName { get; set; }

        /// <summary>
        /// 隨機標記
        /// </summary>
        [ConcurrencyCheck]
        public override string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 使用者與角色之間的連結
        /// </summary>
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
        
        /// <summary>
        /// 角色聲明
        /// </summary>
        public ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
    }
}