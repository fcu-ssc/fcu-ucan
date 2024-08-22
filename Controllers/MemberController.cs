using ClosedXML.Excel;
using fcu_ucan.Data;
using fcu_ucan.Entities;
using fcu_ucan.Models;
using fcu_ucan.Models.Member;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fcu_ucan.Controllers;

[Authorize]
[Route("manage/members")]
public class MemberController(ApplicationDbContext dbContext) : Controller
{
    /// <summary>
    /// 成員頁面
    /// </summary>
    [HttpGet("")]
    public async Task<ActionResult<PaginatedList<MemberViewModel>>> Index([FromQuery] int? page, [FromQuery] string search)
    {
        var query = dbContext.Members.AsNoTracking();
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(x => x.NetworkId.Contains(search) || x.StudentId.Contains(search));
        }
        var entities = await query
            .OrderBy(x => x.Id)
            .Skip((page ?? 1 - 1) * 50)
            .Take(50)
            .ToListAsync();
        var count = await query.CountAsync();
        var models = entities.Select(e => new MemberViewModel
        {
           Id = e.Id,
           NetworkId = e.NetworkId,
           StudentId = e.StudentId
        }).ToList();
        var paginatedModels = new PaginatedList<MemberViewModel>(models, count, page ?? 1, 50);
        return View(paginatedModels);
    }
    
    /// <summary>
    /// 成員詳情頁面
    /// </summary>
    [HttpGet("{memberId}")]
    public async Task<ActionResult<MemberViewModel>> Detail([FromRoute] string memberId)
    {
        var entity = await dbContext.Members
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == memberId);
        if (entity is null)
        {
            return NotFound();
        }
        
        var model = new MemberViewModel
        {
            Id = entity.Id,
            NetworkId = entity.NetworkId,
            StudentId = entity.StudentId
        };
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
            if (await dbContext.Members.AnyAsync(x => x.NetworkId == model.NetworkId))
            {
                ModelState.AddModelError(nameof(model.NetworkId), "NID 帳號已經被使用");
            }
            if (await dbContext.Members.AnyAsync(x => x.StudentId == model.StudentId))
            {
                ModelState.AddModelError(nameof(model.StudentId), "UCAN 帳號已經被使用");
            }
            if (ModelState.IsValid)
            {
                var entity = new Member
                {
                    NetworkId = model.NetworkId,
                    StudentId = model.StudentId
                };
                await dbContext.Members.AddAsync(entity);
                await dbContext.SaveChangesAsync();
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
            await dbContext.Members.AddRangeAsync(entities);
            await dbContext.SaveChangesAsync();
        }
        return RedirectToAction("Index", "Member");
    }
    
    /// <summary>
    /// 編輯成員頁面
    /// </summary>
    [HttpGet("{memberId}/edit")]
    public async Task<ActionResult<MemberEditViewModel>> Edit([FromRoute] string memberId)
    {
        var entity = await dbContext.Members
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == memberId);
        if (entity == null)
        {
            return NotFound();
        }

        var model = new MemberEditViewModel
        {
            NetworkId = entity.NetworkId,
            StudentId = entity.StudentId
        };
        return View(model);
    }
    
    /// <summary>
    /// 編輯成員
    /// </summary>
    [HttpPost("{memberId}/edit")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<MemberEditViewModel>> Edit([FromRoute] string memberId, [FromForm] MemberEditViewModel model)
    {
        var entity = await dbContext.Members
            .SingleOrDefaultAsync(x => x.Id == memberId);
        if (entity == null)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            if (entity.NetworkId != model.NetworkId)
            {
                if (await dbContext.Members.AnyAsync(x => x.NetworkId == model.NetworkId))
                {
                    ModelState.AddModelError(nameof(model.NetworkId), "NID 帳號已經被使用");
                }
            }
            if (entity.StudentId != model.StudentId)
            {
                if (await dbContext.Members.AnyAsync(x => x.StudentId == model.StudentId))
                {
                    ModelState.AddModelError(nameof(model.StudentId), "UCAN 帳號已經被使用");
                }
            }
            if (ModelState.IsValid)
            {
                entity.NetworkId = model.NetworkId;
                entity.StudentId = model.StudentId;
                dbContext.Members.Update(entity);
                await dbContext.SaveChangesAsync();
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
        var entity = await dbContext.Members
            .SingleOrDefaultAsync(x => x.Id == memberId);
        if (entity == null)
        {
            return NotFound();
        }
        dbContext.Members.Remove(entity);
        await dbContext.SaveChangesAsync();
        return RedirectToAction("Index", "Member");
    }
}