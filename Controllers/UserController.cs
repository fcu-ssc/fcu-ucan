using System.Security.Claims;
using AutoMapper;
using fcu_ucan.Data;
using fcu_ucan.Entities;
using fcu_ucan.Helpers;
using fcu_ucan.Models;
using fcu_ucan.Models.User;
using fcu_ucan.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fcu_ucan.Controllers;

[AuthAuthorize(Roles = "User")]
[Route("manage/users")]
public class UserController(
    ILogger<UserController> logger,
    ApplicationDbContext dbContext,
    IMapper mapper,
    UserManager<ApplicationUser> userManager,
    IMailService mailService) : Controller
{
    /// <summary>
    /// 使用者頁面
    /// </summary>
    [HttpGet("")]
    public async Task<ActionResult<PaginatedList<UserViewModel>>> Index([FromQuery] int? page, [FromQuery] string search)
    {
        var query = dbContext.Users.AsNoTracking();
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(x => x.NormalizedUserName!.Contains(search.ToUpperInvariant()) || 
                                     x.NormalizedEmail!.Contains(search.ToUpperInvariant()));
        }
        var entities = await query
            .Include(x => x.UserRoles!)
            .ThenInclude(x => x.Role)
            .Skip((page ?? 1 - 1) * 50)
            .Take(50)
            .ToListAsync();
        var count = await query.CountAsync();
        var models = mapper.Map<List<UserViewModel>>(entities);
        var paginatedModels = new PaginatedList<UserViewModel>(models, count, page ?? 1, 50);
        return View(paginatedModels);
    }
    
    /// <summary>
    /// 使用者詳情頁面
    /// </summary>
    [HttpGet("{userId}")]
    public async Task<ActionResult<UserViewModel>> Detail([FromRoute] string userId)
    {
        var entity = await dbContext.Users
            .AsNoTracking()
            .Include(x => x.UserRoles!)
            .ThenInclude(x => x.Role)
            .SingleOrDefaultAsync(x => x.Id == userId);
        if (entity == null)
        {
            return NotFound();
        }
        var model = mapper.Map<UserViewModel>(entity);
        return View(model);
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
    public async Task<ActionResult<UserInviteViewModel>> Invite([FromForm] UserInviteViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (await dbContext.Users.AnyAsync(x => x.NormalizedEmail == model.Email.ToUpperInvariant()))
            {
                ModelState.AddModelError("Email", "電子郵件已經被使用");
            }
            if (ModelState.IsValid)
            {
                var entity = mapper.Map<ApplicationUser>(model);
                await userManager.CreateAsync(entity);
                if (model.IsRecorder)
                {
                    await userManager.AddToRoleAsync(entity, "Recorder");
                }
                if (model.IsMember)
                {
                    await userManager.AddToRoleAsync(entity, "Member");
                }
                if (model.IsUser)
                {
                    await userManager.AddToRoleAsync(entity, "User");
                }
                if (model.IsUCAN)
                {
                    await userManager.AddToRoleAsync(entity, "UCAN");
                }
                await mailService.SendRegisterEmailAsync(model.Email, entity.SecurityStamp!);
                logger.LogInformation($"{model.Email} 註冊邀請 {entity.SecurityStamp}");
                return RedirectToAction("Index", "User");
            }
        }
        return View(model);
    }
    
    /// <summary>
    /// 編輯使用者頁面
    /// </summary>
    [HttpGet("{userId}/edit")]
    public async Task<ActionResult<UserEditViewModel>> Edit([FromRoute] string userId)
    {
        var entity = await dbContext.Users
            .AsNoTracking()
            .Include(x => x.UserRoles!)
            .ThenInclude(x => x.Role)
            .SingleOrDefaultAsync(x => x.Id == userId);
        if (entity == null)
        {
            return NotFound();
        }
        var model = mapper.Map<UserEditViewModel>(entity);
        return View(model);
    }
    
    /// <summary>
    /// 編輯使用者
    /// </summary>
    [HttpPost("{userId}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<UserEditViewModel>> Edit([FromRoute] string userId, [FromForm] UserEditViewModel model)
    {
        var entity = await dbContext.Users
            .SingleOrDefaultAsync(x => x.Id == userId);
        if (entity == null)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            if (model.Email != entity.Email)
            {
                if (await dbContext.Users.AnyAsync(x => x.NormalizedEmail == model.Email.ToUpperInvariant()))
                {
                    ModelState.AddModelError("Email", "電子郵件已經被使用");
                }
            }
            if (string.IsNullOrEmpty(model.PhoneNumber) && model.PhoneNumber != entity.PhoneNumber)
            {
                if (await dbContext.Users.AnyAsync(x => x.PhoneNumber == model.PhoneNumber))
                {
                    ModelState.AddModelError("PhoneNumber", "手機號碼已經被使用");
                }
            }
            if (ModelState.IsValid)
            {
                var updateEntity = mapper.Map(model, entity);
                await userManager.UpdateAsync(updateEntity);
                var isRecorder = await userManager.IsInRoleAsync(entity, "Recorder");
                if (isRecorder != model.IsRecorder)
                {
                    if (model.IsRecorder)
                    {
                        await userManager.AddToRoleAsync(entity, "Recorder");
                    }
                    else
                    {
                        await userManager.RemoveFromRoleAsync(entity, "Recorder");
                    }
                }
                var isMember = await userManager.IsInRoleAsync(entity, "Member");
                if  (isMember != model.IsMember)
                {
                    if (model.IsMember)
                    {
                        await userManager.AddToRoleAsync(entity, "Member");
                    }
                    else
                    {
                        await userManager.RemoveFromRoleAsync(entity, "Member");
                    }
                }
                var isUser = await userManager.IsInRoleAsync(entity, "User");
                if (isUser != model.IsUser)
                {
                    if (model.IsUser)
                    {
                        await userManager.AddToRoleAsync(entity, "User");
                    }
                    else
                    {
                        await userManager.RemoveFromRoleAsync(entity, "User");
                    }
                }
                var isUCAN = await userManager.IsInRoleAsync(entity, "UCAN");
                if (isUCAN != model.IsUCAN)
                {
                    if (model.IsUCAN)
                    {
                        await userManager.AddToRoleAsync(entity, "UCAN");
                    }
                    else
                    {
                        await userManager.RemoveFromRoleAsync(entity, "UCAN");
                    }
                }
                await userManager.UpdateSecurityStampAsync(entity);
                var currentUserId = User.Claims
                    .Single(p => p.Type == ClaimTypes.NameIdentifier).Value;
                if (currentUserId == entity.Id)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Login", "Account");
                }
                return RedirectToAction("Detail", "User", new{ userId });
            }
        }
        return View(model);
    }
    
    /// <summary>
    /// 刪除使用者
    /// </summary>
    [HttpPost("{userId}/delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromRoute] string userId)
    {
        var entity = await dbContext.Users
            .SingleOrDefaultAsync(x => x.Id == userId);
        if (entity == null)
        {
            return NotFound();
        }
        await userManager.DeleteAsync(entity);
        return RedirectToAction("Index", "User");
    }
}