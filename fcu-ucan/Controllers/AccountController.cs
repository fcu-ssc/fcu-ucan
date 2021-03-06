using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using fcu_ucan.Entities;
using fcu_ucan.Helpers;
using fcu_ucan.Models.Account;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace fcu_ucan.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;

        public AccountController(
            ILogger<AccountController> logger, 
            IConfiguration configuration, 
            IWebHostEnvironment environment, 
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            RoleManager<ApplicationRole> roleManager, 
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _environment = environment;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }
        
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

                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    ModelState.AddModelError("", "登入失敗，請檢查您的帳號密碼是否正確");
                    _logger.LogInformation($"{model.UserName} 登入失敗");
                    return View(model);
                }
                if (!user.EmailConfirmed)
                {
                    ModelState.AddModelError("", "帳戶尚未驗證，請前往您的信箱收取驗證信");
                    _logger.LogInformation($"{model.UserName} 帳戶尚未驗證");
                    return View(model);
                }
                if (!user.IsEnable)
                {
                    ModelState.AddModelError("", "帳戶尚未啟用，請聯絡管理員");
                    _logger.LogInformation($"{model.UserName} 帳戶尚未啟用");
                    return View(model);
                }

                #endregion
                
                #region 檢查密碼
                
                var checkPasswordResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, true);
                if (checkPasswordResult.IsLockedOut)
                {
                    ModelState.AddModelError("", "帳戶被鎖定，請聯絡管理員");
                    _logger.LogInformation($"{model.UserName} 帳戶被鎖定");
                    return View(model);
                }
                if (checkPasswordResult.IsNotAllowed)
                {
                    ModelState.AddModelError("", "帳戶尚未驗證，請前往您的信箱收取驗證信");
                    _logger.LogInformation($"{model.UserName} 帳戶尚未驗證");
                    return View(model);
                }
                if (checkPasswordResult.Succeeded)
                {
                    #region 添加角色聲明
                    
                    var claims = await _userManager.GetClaimsAsync(user);
                    var roleNames = await _userManager.GetRolesAsync(user);
                    foreach (var roleName in roleNames)
                    {
                        var role = await _roleManager.FindByNameAsync(roleName);
                        var roleClaims = await _roleManager.GetClaimsAsync(role);
                        foreach (var roleClaim in roleClaims)
                        {
                            claims.Add(roleClaim);
                        }
                    }
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    claims.Add(new Claim(ClaimTypes.Sid, user.SecurityStamp));

                    #endregion
                    
                    var token = GenerateJwtToken(claims);
                    HttpContext.Session.SetString("token", token);
                    _logger.LogInformation($"{model.UserName} 登入成功 {token}");
                    return RedirectToAction("Index", "Manage");
                }    

                #endregion
            }
            ModelState.AddModelError("", "登入失敗，請檢查您的帳號密碼是否正確");
            _logger.LogInformation($"{model.UserName} 登入失敗");
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
            var entity = await _userManager.Users
                .SingleOrDefaultAsync(x => x.SecurityStamp == code);
            if (entity == null)
            {
                _logger.LogInformation($"{code} 無效");
                return NotFound();
            }
            if (entity.IsEnable)
            {
                _logger.LogInformation($"帳戶已經啟用，禁止註冊");
                return NotFound();
            }
            if (entity.NormalizedUserName != Regex.Replace(entity.Id, "[^A-Za-z0-9]", "").ToUpperInvariant())
            {
                _logger.LogInformation($"{entity.Id} {entity.UserName} {entity.NormalizedUserName} 已經被註冊");
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
            var entity = await _userManager.Users
                .SingleOrDefaultAsync(x => x.SecurityStamp == code);
            if (entity == null)
            {
                _logger.LogInformation($"{code} 無效");
                return NotFound();
            }
            if (entity.IsEnable)
            {
                _logger.LogInformation($"帳戶已經啟用，禁止註冊");
                return NotFound();
            }
            if (entity.NormalizedUserName != Regex.Replace(entity.Id, "[^A-Za-z0-9]", "").ToUpperInvariant())
            {
                _logger.LogInformation($"{entity.Id} {entity.UserName} {entity.NormalizedUserName} 已經被註冊");
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (await _userManager.Users.AnyAsync(x => x.NormalizedUserName == model.UserName.ToUpperInvariant()))
                {
                    ModelState.AddModelError("UserName", "使用者名稱已經被使用");
                    return View(model);
                }
                await _userManager.SetUserNameAsync(entity, model.UserName);
                await _userManager.AddPasswordAsync(entity, model.Password);
                var updateEntity = _mapper.Map(model, entity);
                await _userManager.UpdateAsync(updateEntity);
                _logger.LogInformation($"{model.UserName} 註冊成功");
                return RedirectToAction("Login", "Account");
            }
            ModelState.AddModelError("", "註冊失敗");
            _logger.LogInformation($"{model.UserName} 註冊失敗");
            return View(model);
        }
        
        [NonAction]
        private string GenerateJwtToken(IList<Claim> claims)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            
            var userClaimsIdentity = new ClaimsIdentity(claims);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var signingCredentials = new SigningCredentials(securityKey, _environment.IsDevelopment() ? SecurityAlgorithms.HmacSha256 : SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
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
}