using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClosedXML.Excel;
using fcu_ucan.Data;
using fcu_ucan.Entities;
using fcu_ucan.Helpers;
using fcu_ucan.Models;
using fcu_ucan.Models.Member;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace fcu_ucan.Controllers
{
    [AuthAuthorize(Roles = "Member")]
    [Route("manage/members")]
    public class MemberController : Controller
    {
        private readonly ILogger<MemberController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public MemberController(ILogger<MemberController> logger, ApplicationDbContext dbContext, IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        /// <summary>
        /// 成員頁面
        /// </summary>
        [HttpGet("")]
        public async Task<ActionResult<PaginatedList<MemberViewModel>>> Index([FromQuery] int? page, [FromQuery] string search)
        {
            var query = _dbContext.Members.AsNoTracking();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.NetworkId.Contains(search) || 
                                         x.StudentId.Contains(search));
            }
            var entities = await query
                .Skip((page ?? 1 - 1) * 50)
                .Take(50)
                .ToListAsync();
            var count = await query.CountAsync();
            var models = _mapper.Map<List<MemberViewModel>>(entities);
            var paginatedModels = new PaginatedList<MemberViewModel>(models, count, page ?? 1, 50);
            return View(paginatedModels);
        }
        
        /// <summary>
        /// 成員詳情頁面
        /// </summary>
        [HttpGet("{memberId}")]
        public async Task<ActionResult<MemberViewModel>> Detail([FromRoute] string memberId)
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
        [HttpGet("add")]
        public IActionResult Add() => View();

        /// <summary>
        /// 新增成員
        /// </summary>
        [HttpPost("add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([FromForm] MemberAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _dbContext.Members.AnyAsync(x => x.NetworkId == model.NetworkId))
                {
                    ModelState.AddModelError("NetworkId", "NID 帳號已經被使用");
                }
                if (await _dbContext.Members.AnyAsync(x => x.StudentId == model.StudentId))
                {
                    ModelState.AddModelError("StudentId", "UCAN 帳號已經被使用");
                }
                if (ModelState.IsValid)
                {
                    var entity = _mapper.Map<Member>(model);
                    await _dbContext.Members.AddAsync(entity);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("Detail", "Member", new{ memberId = entity.Id });
                }
            }
            return View(model);
        }
        
        /// <summary>
        /// 匯入成員
        /// </summary>
        [HttpPost("import")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            using (var wbook = new XLWorkbook(file.OpenReadStream()))
            {
                var worksheet = wbook.Worksheet(1);
                var entities = new List<Member>();
                foreach (IXLRow row in worksheet.Rows())
                {
                    entities.Add(new Member
                    {
                        NetworkId = row.Cell(1).Value.ToString(),
                        StudentId = row.Cell(2).Value.ToString()
                    });
                }
                await _dbContext.Members.AddRangeAsync(entities);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Member");
        }

        /// <summary>
        /// 編輯成員頁面
        /// </summary>
        [HttpGet("{memberId}/edit")]
        public async Task<ActionResult<MemberEditViewModel>> Edit([FromRoute] string memberId)
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
        [HttpPost("{memberId}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<MemberEditViewModel>> Edit([FromRoute] string memberId, [FromForm] MemberEditViewModel model)
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
                        ModelState.AddModelError("NetworkId", "NID 帳號已經被使用");
                    }
                }
                if (entity.StudentId != model.StudentId)
                {
                    if (await _dbContext.Members.AnyAsync(x => x.StudentId == model.StudentId))
                    {
                        ModelState.AddModelError("StudentId", "UCAN 帳號已經被使用");
                    }
                }
                if (ModelState.IsValid)
                {
                    var updateEntity = _mapper.Map(model, entity);
                    _dbContext.Members.Update(updateEntity);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("Detail", "Member", new{ memberId });
                }
            }
            return View(model);
        }
        
        /// <summary>
        /// 刪除成員
        /// </summary>
        [HttpPost("{memberId}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string memberId)
        {
            var entity = await _dbContext.Members
                .SingleOrDefaultAsync(x => x.Id == memberId);
            if (entity == null)
            {
                return NotFound();
            }
            _dbContext.Members.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Member");
        }
    }
}