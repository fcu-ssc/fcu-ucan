using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using AutoMapper;
using fcu_ucan.Entities;
using fcu_ucan.Helpers;
using fcu_ucan.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace fcu_ucan.Controllers;

[Route("account")]
public class AccountController(
    ILogger<AccountController> logger,
    IConfiguration configuration,
    IWebHostEnvironment environment,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    RoleManager<ApplicationRole> roleManager,
    IMapper mapper) : Controller
{
    /// <summary>
    /// 登入頁面
    /// </summary>
    [HttpGet("login")]
    public IActionResult Login() => View();
    
    /// <summary>
    /// 登入
    /// </summary>
    [HttpPost("login")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<LoginViewModel>> Login([FromForm] LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            #region 檢查是否可登入

            var user = await userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "登入失敗，請檢查您的帳號密碼是否正確");
                logger.LogInformation($"{model.UserName} 登入失敗");
                return View(model);
            }
            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "帳戶尚未驗證，請前往您的信箱收取驗證信");
                logger.LogInformation($"{model.UserName} 帳戶尚未驗證");
                return View(model);
            }
            if (!user.IsEnable)
            {
                ModelState.AddModelError("", "帳戶尚未啟用，請聯絡管理員");
                logger.LogInformation($"{model.UserName} 帳戶尚未啟用");
                return View(model);
            }

            #endregion
            
            #region 檢查密碼
            
            var checkPasswordResult = await signInManager.CheckPasswordSignInAsync(user, model.Password, true);
            if (checkPasswordResult.IsLockedOut)
            {
                ModelState.AddModelError("", "帳戶被鎖定，請聯絡管理員");
                logger.LogInformation($"{model.UserName} 帳戶被鎖定");
                return View(model);
            }
            if (checkPasswordResult.IsNotAllowed)
            {
                ModelState.AddModelError("", "帳戶尚未驗證，請前往您的信箱收取驗證信");
                logger.LogInformation($"{model.UserName} 帳戶尚未驗證");
                return View(model);
            }
            if (checkPasswordResult.Succeeded)
            {
                #region 添加角色聲明
                
                var claims = await userManager.GetClaimsAsync(user);
                var roleNames = await userManager.GetRolesAsync(user);
                foreach (var roleName in roleNames)
                {
                    var role = await roleManager.FindByNameAsync(roleName);
                    var roleClaims = await roleManager.GetClaimsAsync(role!);
                    foreach (var roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                claims.Add(new Claim(ClaimTypes.Sid, user.SecurityStamp!));

                #endregion
                
                var token = GenerateJwtToken(claims);
                HttpContext.Session.SetString("token", token);
                logger.LogInformation($"{model.UserName} 登入成功 {token}");
                return RedirectToAction("Index", "Manage");
            }    

            #endregion
        }
        ModelState.AddModelError("", "登入失敗，請檢查您的帳號密碼是否正確");
        logger.LogInformation($"{model.UserName} 登入失敗");
        return View(model);
    }
    
    /// <summary>
    /// 登出
    /// </summary>
    [AuthAuthorize]
    [HttpGet("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
    
    /// <summary>
    /// 註冊頁面
    /// </summary>
    [HttpGet("register/{code}")]
    public async Task<IActionResult> Register([FromRoute] string code)
    {
        var entity = await userManager.Users
            .SingleOrDefaultAsync(x => x.SecurityStamp == code);
        if (entity == null)
        {
            logger.LogInformation($"{code} 無效");
            return NotFound();
        }
        if (entity.IsEnable)
        {
            logger.LogInformation($"帳戶已經啟用，禁止註冊");
            return NotFound();
        }
        if (entity.NormalizedUserName != Regex.Replace(entity.Id, "[^A-Za-z0-9]", "").ToUpperInvariant())
        {
            logger.LogInformation($"{entity.Id} {entity.UserName} {entity.NormalizedUserName} 已經被註冊");
            return NotFound();
        }
        return View();
    }
    
    /// <summary>
    /// 註冊
    /// </summary>
    [HttpPost("register/{code}")]
    public async Task<ActionResult<RegisterViewModel>> Register([FromRoute] string code, [FromForm] RegisterViewModel model)
    {
        var entity = await userManager.Users
            .SingleOrDefaultAsync(x => x.SecurityStamp == code);
        if (entity == null)
        {
            logger.LogInformation($"{code} 無效");
            return NotFound();
        }
        if (entity.IsEnable)
        {
            logger.LogInformation($"帳戶已經啟用，禁止註冊");
            return NotFound();
        }
        if (entity.NormalizedUserName != Regex.Replace(entity.Id, "[^A-Za-z0-9]", "").ToUpperInvariant())
        {
            logger.LogInformation($"{entity.Id} {entity.UserName} {entity.NormalizedUserName} 已經被註冊");
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            if (await userManager.Users.AnyAsync(x => x.NormalizedUserName == model.UserName.ToUpperInvariant()))
            {
                ModelState.AddModelError("UserName", "使用者名稱已經被使用");
                return View(model);
            }
            await userManager.SetUserNameAsync(entity, model.UserName);
            await userManager.AddPasswordAsync(entity, model.Password);
            var updateEntity = mapper.Map(model, entity);
            await userManager.UpdateAsync(updateEntity);
            logger.LogInformation($"{model.UserName} 註冊成功");
            return RedirectToAction("Login", "Account");
        }
        ModelState.AddModelError("", "註冊失敗");
        logger.LogInformation($"{model.UserName} 註冊失敗");
        return View(model);
    }
    
    [NonAction]
    private string GenerateJwtToken(IList<Claim> claims)
    {
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            
        var userClaimsIdentity = new ClaimsIdentity(claims);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!));
        var signingCredentials = new SigningCredentials(securityKey, environment.IsDevelopment() ? SecurityAlgorithms.HmacSha256 : SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = configuration["JWT:Issuer"],
            Audience = configuration["JWT:Audience"],
            Subject = userClaimsIdentity,
            NotBefore = DateTime.UtcNow, // Token 在什麼時間之前，不可用
            IssuedAt = DateTime.UtcNow, // Token 的建立時間
            Expires = DateTime.Now.AddHours(8), // Token 的逾期時間
            SigningCredentials = signingCredentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var serializeToken = tokenHandler.WriteToken(securityToken);
        return serializeToken;
    }
}