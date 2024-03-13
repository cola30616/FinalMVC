using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.DTO;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FinalGroupMVCPrj.Controllers
{
    [AllowAnonymous]
    public class AdminMemberController : Controller
    {
        private readonly LifeShareLearnContext _context;
        public AdminMemberController(LifeShareLearnContext context) 
        {
            _context = context;
        }
        public IActionResult List()
        {
            return View();
        }
        public IActionResult ListDataJson([FromBody] MemberFilterDTO? memberFilterData)
        {
            var memberList = _context.TMembers.AsQueryable();
            if (memberFilterData != null)
            {

            }
            IEnumerable<MemberBasicViewModel> mBasicVMCollection =
                  new List<MemberBasicViewModel>(
                            memberList.Select(m => new MemberBasicViewModel
                            {
                                MemberId = m.FMemberId,
                                Email = m.FEmail,
                                EmailVerification = m.FEmailVerification ? "已驗證" : "未驗證",
                                RealName = m.FRealName,
                                ShowName = m.FShowName,
                                GetCampInfo = m.FGetCampaignInfo ? "是" : "否",
                                RegisterDateTime = m.FRegisterDatetime.ToString("yyyy/MM/dd HH:mm"),
                                Status = m.FStatus == true ? "正常" : "停權中"
                            })); ;
            return Json(new{data = mBasicVMCollection});
        }
        public IActionResult LoginHistory(int memberId, int toSkip, DateTime? lastDate)
        {
            var q = _context.TMemberLoginLogs.Where(l => l.FMemberId == memberId);
            if(lastDate != null)
            {
                q = q.Where(l => l.FLoginDateTime <= lastDate);
            }

            var   loginHistory =  q.OrderByDescending(m=>m.FLoginDateTime).Skip(toSkip).Take(5).ToList();

            return Json(loginHistory);
        }
        // GET: AdminMember/Detail/1
        //動作簡述：回傳會員詳細資訊 AdminMemberDTO
        [HttpGet]
        public IActionResult Detail(int id)
        {
            int memberId = id;
            if (memberId == 0) { return BadRequest("沒有指定會員"); };
            var dbMember = _context.TMembers.Include(m => m.TMemberCitiesLists).Include(m => m.TMemberWishFields).FirstOrDefault(m => m.FMemberId == memberId);
            if (dbMember == null) { return StatusCode(500,"資料庫系統異常"); };
            try
            {
                AdminMemberDTO mdVm = new AdminMemberDTO
                {
                    MemberId = dbMember.FMemberId,
                    RealName = dbMember.FRealName,
                    ShowName = dbMember.FShowName,
                    Email = dbMember.FEmail,
                    EmailConfirmed = dbMember.FEmailVerification?"已驗證":"未驗證",
                    Phone = dbMember.FPhone,
                    GetCampInfo = dbMember.FGetCampaignInfo?"是":"否",
                    Birth = dbMember.FBirthDate?.ToString("yyyy/MM/dd"),
                    Gender = dbMember.FGender == true ? "男" : (dbMember.FGender == false ? "女" : "其他／不便透露"),
                    Job = dbMember.FJob,
                    Education = dbMember.FEducation,
                    Note = string.IsNullOrEmpty(dbMember.FNote)?"未連結任何帳號":"LINE" ,
                    Cities =
                    dbMember.TMemberCitiesLists.Join(
                        _context.TCities, m => m.FCityId, c => c.FCityId,
                        (m, c) => c.FCityName),
                    WishFields =
                                        dbMember.TMemberWishFields.Join(
                        _context.TCourseFields, m => m.FFieldId, f => f.FFieldId,
                        (m, f) => f.FFieldName)
                };
            return Ok(mdVm );
            }catch (Exception ex)
            {
                return StatusCode(500, "系統異常："+ex);
            }

        }
    }
}
