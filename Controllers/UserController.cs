using System.Text;
using fcu_ucan.Models;
using fcu_ucan.Models.User;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace fcu_ucan.Controllers;

[Authorize]
[Route("manage/users")]
public class UserController(IConfiguration configuration, UserManager<IdentityUser> userManager) : Controller
{
    /// <summary>
    /// 使用者頁面
    /// </summary>
    [HttpGet("")]
    public async Task<ActionResult<PaginatedList<UserViewModel>>> IndexAsync([FromQuery] int? page)
    {
        var entities = await userManager.Users.AsNoTracking()
            .OrderBy(x => x.Id)
            .Skip((page ?? 1 - 1) * 50)
            .Take(50)
            .ToListAsync();
        var count = await userManager.Users.CountAsync();
        var models = entities.Select(e => new UserViewModel
        {
            Id = e.Id,
            UserName = e.UserName!,
            Email = e.Email!,
            EmailConfirmed = e.EmailConfirmed
        }).ToList();
        var paginatedModels = new PaginatedList<UserViewModel>(models, count, page ?? 1, 50);
        return View(paginatedModels);
    }
    
    /// <summary>
    /// 使用者詳情頁面
    /// </summary>
    [HttpGet("{userId}")]
    public async Task<ActionResult<UserViewModel>> DetailAsync([FromRoute] string userId)
    {
        var entity = await userManager.FindByIdAsync(userId);
        if (entity is null)
        {
            return NotFound();
        }
        var model = new UserViewModel
        {
            Id = entity.Id,
            UserName = entity.UserName!,
            Email = entity.Email!,
            EmailConfirmed = entity.EmailConfirmed
        };
        return View(model);
    }
    
    /// <summary>
    /// 編輯使用者頁面
    /// </summary>
    [HttpGet("{userId}/edit")]
    public async Task<ActionResult<UserEditViewModel>> EditAsync([FromRoute] string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return NotFound();
        }

        var model = new UserEditViewModel
        {
            Email = user.Email!,
            EmailConfirmed = user.EmailConfirmed
        };
        return View(model);
    }
    
    /// <summary>
    /// 編輯使用者
    /// </summary>
    [HttpPost("{userId}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<UserEditViewModel>> EditAsync([FromRoute] string userId, [FromForm] UserEditViewModel model)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return NotFound();
        }
        
        if (ModelState.IsValid)
        {
            if (model.Email != user.Email)
            {
                var existEmail = await userManager.FindByEmailAsync(model.Email);
                if (existEmail is not null)
                {
                    ModelState.AddModelError(nameof(model.Email), "電子郵件已經被使用");
                    return View(model);
                }
            }
            
            user.Email = model.Email;
            user.EmailConfirmed = model.EmailConfirmed;
            await userManager.UpdateAsync(user);
            
            return RedirectToAction(controllerName: "User", actionName: "Detail", routeValues: new{ userId });
        }
        return View(model);
    }
    
    /// <summary>
    /// 刪除使用者
    /// </summary>
    [HttpPost("{userId}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAsync([FromRoute] string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return NotFound();
        }
        
        await userManager.DeleteAsync(user);
        return RedirectToAction(controllerName: "User", actionName: "Index");
    }
    
    /// <summary>
    /// 邀請使用者頁面
    /// </summary>
    [HttpGet("invite")]
    public IActionResult Invite() => View();
    
    /// <summary>
    /// 邀請使用者
    /// </summary>
    [HttpPost("invite")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<UserInviteViewModel>> InviteAsync([FromForm] UserInviteViewModel model)
    {
        if (ModelState.IsValid)
        {
            var existUserName = await userManager.FindByNameAsync(model.UserName);
            if (existUserName is not null)
            {
                ModelState.AddModelError(nameof(model.UserName), "使用者名稱已經被使用");
            }
            
            var existEmail = await userManager.FindByEmailAsync(model.Email);
            if (existEmail is not null)
            {
                ModelState.AddModelError(nameof(model.Email), "電子郵件已經被使用");
            }
            
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.UserName,
                    Email = model.Email
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var url = Url.ActionLink(controller: "Account", action: "ConfirmEmail", values: new
                    {
                        id = user.Id, 
                        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token))
                    })!;
                    
                    var message = new MimeMessage {Importance = MessageImportance.High};
                    message.From.Add(new MailboxAddress(
                        name: configuration.GetSection("Mail").GetValue<string>("SenderName"), 
                        address: configuration.GetSection("Mail").GetValue<string>("SenderEmail")));
                    message.To.Add(new MailboxAddress(name: model.UserName, address: model.Email));
                    message.Subject = "FCU x UCAN 電子郵件驗證信";
                    
                    var bodyBuilder = new BodyBuilder
                    {
                        HtmlBody = $"""<p>請點擊下方連結註冊</p><a href="{url}">{url}</a>"""
                    };
                    message.Body = bodyBuilder.ToMessageBody();
                    
                    using var client = new SmtpClient();
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    await client.ConnectAsync(
                        host: configuration.GetSection("Mail").GetValue<string>("Server"), 
                        port: configuration.GetSection("Mail").GetValue<int>("Port"), 
                        options: SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(
                        userName: configuration.GetSection("Mail").GetValue<string>("UserName"), 
                        password: configuration.GetSection("Mail").GetValue<string>("Password"));
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                
                    return RedirectToAction(controllerName: "User", actionName: "Index");
                }
                
                ModelState.AddModelError(string.Empty, "發生錯誤，請稍候再試");
            }
        }
        return View(model);
    }
}