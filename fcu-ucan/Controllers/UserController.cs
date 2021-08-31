using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using fcu_ucan.Data;
using fcu_ucan.Entities;
using fcu_ucan.Helpers;
using fcu_ucan.Models;
using fcu_ucan.Models.User;
using fcu_ucan.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace fcu_ucan.Controllers
{
    [AuthAuthorize(Roles = "User")]
    [Route("manage/users")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMailService _mailService;

        public UserController(
            ILogger<UserController> logger, 
            ApplicationDbContext dbContext, 
            IMapper mapper, 
            UserManager<ApplicationUser> userManager, 
            IMailService mailService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
            _mailService = mailService;
        }
        
        /// <summary>
        /// 使用者頁面
        /// </summary>
        [HttpGet("")]
        public async Task<ActionResult<PaginatedList<UserViewModel>>> Index([FromQuery] int? page)
        {
            var entities = await _dbContext.Users
                .AsNoTracking()
                .Skip((page ?? 1 - 1) * 50)
                .Take(30)
                .ToListAsync();
            var count = await _dbContext.Users.CountAsync();
            var models = _mapper.Map<List<UserViewModel>>(entities);
            var paginatedModels = new PaginatedList<UserViewModel>(models, count, page ?? 1, 50);
            return View(paginatedModels);
        }
        
        /// <summary>
        /// 使用者詳情頁面
        /// </summary>
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserViewModel>> Detail([FromRoute] string userId)
        {
            var entity = await _dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == userId);
            if (entity == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<UserViewModel>(entity);
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
                if (await _dbContext.Users.AnyAsync(x => x.NormalizedEmail == model.Email.ToUpperInvariant()))
                {
                    ModelState.AddModelError("Email", "電子郵件已經被使用");
                }
                if (ModelState.IsValid)
                {
                    var entity = _mapper.Map<ApplicationUser>(model);
                    await _userManager.CreateAsync(entity);
                    if (model.IsRecorder)
                    {
                        await _userManager.AddToRoleAsync(entity, "Recorder");
                    }
                    if (model.IsMember)
                    {
                        await _userManager.AddToRoleAsync(entity, "Member");
                    }
                    if (model.IsUser)
                    {
                        await _userManager.AddToRoleAsync(entity, "User");
                    }
                    await _mailService.SendRegisterEmailAsync(model.Email, entity.SecurityStamp);
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
            var entity = await _dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == userId);
            if (entity == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<UserEditViewModel>(entity);
            return View(model);
        }
        
        /// <summary>
        /// 編輯使用者
        /// </summary>
        [HttpPost("{userId}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<UserEditViewModel>> Edit([FromRoute] string userId, [FromForm] UserEditViewModel model)
        {
            var entity = await _dbContext.Users
                .SingleOrDefaultAsync(x => x.Id == userId);
            if (entity == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (model.Email != entity.Email)
                {
                    if (await _dbContext.Users.AnyAsync(x => x.NormalizedEmail == model.Email.ToUpperInvariant()))
                    {
                        ModelState.AddModelError("Email", "電子郵件已經被使用");
                    }
                }
                if (string.IsNullOrEmpty(model.PhoneNumber) && model.PhoneNumber != entity.PhoneNumber)
                {
                    if (await _dbContext.Users.AnyAsync(x => x.PhoneNumber == model.PhoneNumber))
                    {
                        ModelState.AddModelError("PhoneNumber", "手機號碼已經被使用");
                    }
                }
                if (ModelState.IsValid)
                {
                    var updateEntity = _mapper.Map(model, entity);
                    await _userManager.UpdateAsync(updateEntity);
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
            var entity = await _dbContext.Users
                .SingleOrDefaultAsync(x => x.Id == userId);
            if (entity == null)
            {
                return NotFound();
            }
            await _userManager.DeleteAsync(entity);
            return RedirectToAction("Index", "User");
        }
    }
}