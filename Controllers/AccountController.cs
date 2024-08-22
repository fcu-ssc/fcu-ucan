using fcu_ucan.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace fcu_ucan.Controllers;

[Route("account")]
public class AccountController(
    ILogger<AccountController> logger,
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager) : Controller
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
    public async Task<ActionResult<LoginViewModel>> LoginAsync([FromForm] LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user is not null)
            {
                var result = await signInManager.PasswordSignInAsync(user, model.Password, false, true);
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "帳戶被鎖定，請聯絡管理員");
                    return View(model);
                }
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, "帳戶尚未驗證，請前往您的信箱收取驗證信");
                    return View(model);
                }
                if (result.Succeeded)
                {
                    return RedirectToAction(controllerName: "Manage", actionName: "Index");
                }
            }
        }
        ModelState.AddModelError(string.Empty, "登入失敗，請檢查您的帳號密碼是否正確");
        logger.LogInformation($"{model.UserName} 登入失敗");
        return View(model);
    }
    
    /// <summary>
    /// 登出
    /// </summary>
    [Authorize]
    [HttpGet("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction(controllerName: "Home", actionName: "Index");
    }

    /// <summary>
    /// 確認電子郵件
    /// </summary>
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string id, [FromQuery] string token)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is not null)
        {
            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                TempData["message"] = "電子郵件驗證成功";
                return RedirectToAction(controllerName: "Account", actionName: "Login");
            }
        }
        return NotFound();
    }
}