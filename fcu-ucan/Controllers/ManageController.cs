using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using fcu_ucan.Data;
using fcu_ucan.Entities;
using fcu_ucan.Models.Member;
using fcu_ucan.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MimeTypes;

namespace fcu_ucan.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Route("manage")]
    public class ManageController : Controller
    {
        private readonly ILogger<ManageController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageController(ILogger<ManageController> logger, ApplicationDbContext dbContext, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
        }
        
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
        
        /// <summary>
        /// 日誌頁面
        /// </summary>
        [HttpGet("logs")]
        public ActionResult<FileInfo[]> Logs()
        {
            var di = new DirectoryInfo("Logs");
            var files = di.GetFiles();
            return View(files);
        }
        
        /// <summary>
        /// 日誌下載
        /// </summary>
        /// <param name="fileName"></param>
        [HttpGet("logs/{fileName}")]
        public IActionResult Logs([FromRoute] string fileName)
        {
            var di = new DirectoryInfo("Logs");
            var file = di.GetFiles(fileName)
                .SingleOrDefault(x => x.Name == fileName);
            if (file == null)
            {
                return NotFound();
            }
            return File(System.IO.File.OpenRead(file.FullName), MimeTypeMap.GetMimeType(file.Extension), file.Name);
        }
        
        /// <summary>
        /// 使用者頁面
        /// </summary>
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> Users()
        {
            var entities = await _dbContext.Users
                .AsNoTracking()
                .ToListAsync();
            var models = _mapper.Map<IEnumerable<UserViewModel>>(entities);
            return View(models);
        }
        
        /// <summary>
        /// 使用者詳情頁面
        /// </summary>
        [HttpGet("users/{userId}")]
        public async Task<ActionResult<UserViewModel>> UserDetail([FromRoute] string userId)
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
        /// 編輯使用者頁面
        /// </summary>
        [HttpGet("users/{userId}/edit")]
        public async Task<ActionResult<UserEditViewModel>> UserEdit([FromRoute] string userId)
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
        [HttpPost("users/{userId}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<UserEditViewModel>> UserEdit([FromRoute] string userId, [FromForm] UserEditViewModel model)
        {
            var entity = await _dbContext.Users
                .SingleOrDefaultAsync(x => x.Id == userId);
            if (entity == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.PhoneNumber) && model.PhoneNumber != entity.PhoneNumber)
                {
                    if (await _dbContext.Users.AnyAsync(x => x.PhoneNumber == model.PhoneNumber))
                    {
                        ModelState.AddModelError("", "");
                    }
                }
                if (ModelState.IsValid)
                {
                    var updateEntity = _mapper.Map(model, entity);
                    await _userManager.UpdateAsync(updateEntity);
                    return RedirectToAction("UserDetail", "Manage", new{ userId });
                }
            }
            return View(model);
        }
        
        /// <summary>
        /// 刪除使用者
        /// </summary>
        [HttpPost("users/{userId}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserDelete([FromRoute] string userId)
        {
            var entity = await _dbContext.Users
                .SingleOrDefaultAsync(x => x.Id == userId);
            if (entity == null)
            {
                return NotFound();
            }
            await _userManager.DeleteAsync(entity);
            return RedirectToAction("Users", "Manage");
        }
        
        /// <summary>
        /// 成員頁面
        /// </summary>
        [HttpGet("members")]
        public async Task<ActionResult<IEnumerable<MemberViewModel>>> Members()
        {
            var entities = await _dbContext.Members
                .AsNoTracking()
                .ToListAsync();
            var models = _mapper.Map<IEnumerable<MemberViewModel>>(entities);
            return View(models);
        }
        
        /// <summary>
        /// 成員詳情頁面
        /// </summary>
        [HttpGet("members/{memberId}")]
        public async Task<ActionResult<MemberViewModel>> MemberDetail([FromRoute] string memberId)
        {
            var entity = await _dbContext.Members
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == memberId);
            var model = _mapper.Map<MemberViewModel>(entity);
            return View(model);
        }
        
        /// <summary>
        /// 新增成員頁面
        /// </summary>
        [HttpGet("members/add")]
        public IActionResult MemberAdd() => View();

        /// <summary>
        /// 新增成員
        /// </summary>
        [HttpPost("members/add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MemberAdd([FromForm] MemberAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _dbContext.Members.AnyAsync(x => x.NetworkId == model.NetworkId))
                {
                    ModelState.AddModelError("NetworkId", "");
                }
                if (await _dbContext.Members.AnyAsync(x => x.StudentId == model.StudentId))
                {
                    ModelState.AddModelError("StudentId", "");
                }
                if (ModelState.IsValid)
                {
                    var entity = _mapper.Map<Member>(model);
                    await _dbContext.Members.AddAsync(entity);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("MemberDetail", "Manage", new{ memberId = entity.Id });
                }
            }
            return View(model);
        }

        /// <summary>
        /// 編輯成員頁面
        /// </summary>
        [HttpGet("members/{memberId}/edit")]
        public async Task<ActionResult<MemberEditViewModel>> MemberEdit([FromRoute] string memberId)
        {
            var entity = await _dbContext.Members
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == memberId);
            if (entity == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<MemberEditViewModel>(entity);
            return View(model);
        }
        
        /// <summary>
        /// 編輯成員
        /// </summary>
        [HttpPost("members/{memberId}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<MemberEditViewModel>> MemberEdit([FromRoute] string memberId, [FromForm] MemberEditViewModel model)
        {
            var entity = await _dbContext.Members
                .SingleOrDefaultAsync(x => x.Id == memberId);
            if (entity == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (entity.NetworkId != model.NetworkId)
                {
                    if (await _dbContext.Members.AnyAsync(x => x.NetworkId == model.NetworkId))
                    {
                        ModelState.AddModelError("", "");
                    }
                }
                if (entity.StudentId != model.StudentId)
                {
                    if (await _dbContext.Members.AnyAsync(x => x.StudentId == model.StudentId))
                    {
                        ModelState.AddModelError("", "");
                    }
                }
                if (ModelState.IsValid)
                {
                    var updateEntity = _mapper.Map(model, entity);
                    _dbContext.Members.Update(updateEntity);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("MemberDetail", "Manage", new{ memberId });
                }
            }
            return View(model);
        }
        
        /// <summary>
        /// 刪除成員
        /// </summary>
        [HttpPost("members/{memberId}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MemberDelete([FromRoute] string memberId)
        {
            var entity = await _dbContext.Members
                .SingleOrDefaultAsync(x => x.Id == memberId);
            if (entity == null)
            {
                return NotFound();
            }
            _dbContext.Members.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Members", "Manage");
        }
    }
}