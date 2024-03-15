using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.DTO;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalGroupMVCPrj.Controllers
{
    [AllowAnonymous]
    public class AdminTeacherController : Controller
    {
        private readonly LifeShareLearnContext _context;

        public AdminTeacherController(LifeShareLearnContext context)
        {
            _context = context;
        }

        public IActionResult ApplyList()
        {
            return View();
        }
        public IActionResult ListDataJson([FromBody] TApplyFilterDTO? tApplyFilterDTO)
        {
            var tApplyList = _context.TTeacherApplyLogs.AsQueryable();
            if (tApplyFilterDTO != null)
            {

            }
            IEnumerable<AdminTApplyVM> tApplyVMCollection =
                  new List<AdminTApplyVM>(
                            tApplyList.Select(a => new AdminTApplyVM
                            {
                                ApplyLogId =a.FApplyLogId,
                                ApplyDatetime = a.FApplyDatetime.ToString("yyyy/MM/dd HH:mm"),
                                MemberId =a.FMemberId,
                                TeacherName = a.FTeacherName,
                                RealName = a.FRealName,
                                ProgressStatus = a.FProgressStatus,
                                ReviewDatetime = a.FReviewDatetime!= null?((DateTime) a.FReviewDatetime).ToString("yyyy/MM/dd HH:mm"): "-",
                                Note = a.FNote??"-"
                            })); ;
            return Json(new { data = tApplyVMCollection });
        }
        // GET: AdminTeacher/ApplyDetail/1
        //動作簡述：回傳申請老師詳細資訊 AdminApplyDetailDTO
        [HttpGet]
        public IActionResult ApplyDetail(int id)
        {
            int applyLogId = id;
            if (applyLogId == 0) { return BadRequest("沒有指定申請資料"); };
            var dbApplyLog = _context.TTeacherApplyLogs.Include(a => a.FMember).FirstOrDefault(a => a.FApplyLogId == applyLogId); ;
            if (dbApplyLog == null) { return StatusCode(500, "資料庫系統異常"); };
            try
            {
                AdminApplyDetailDTO applyDVm = new AdminApplyDetailDTO
                {
                    FApplyLogId = applyLogId,
                    FMemberId = dbApplyLog.FMemberId,
                    MemberRealName = dbApplyLog.FMember.FRealName,
                    MemberEmail = dbApplyLog.FMember.FEmail,
                    ApplyDatetime = dbApplyLog.FApplyDatetime.ToString("yyyy/MM/dd HH:mm"),
                    RealName = dbApplyLog.FRealName,
                    ContactInfo = dbApplyLog.FContactInfo,
                    TeacherName = dbApplyLog.FTeacherName,
                    Introduction = dbApplyLog.FIntroduction,
                    Reason = dbApplyLog.FReason,
                    PdfLink = dbApplyLog.FPdfLink,
                    ProgressStatus = dbApplyLog.FProgressStatus,
                    ReviewDatetime = dbApplyLog.FReviewDatetime?.ToString("yyyy/MM/dd HH:mm"),
                    FReviewResult = dbApplyLog.FReviewResult,
                    Note = dbApplyLog.FNote
                };
                return Ok(applyDVm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "系統異常：" + ex);
            }
        }

        //POST:  AdminTeacher/ApplyReview
        [HttpPost]
        public IActionResult ApplyReview(int ApplyLogId, string progerss, string? note, DateTime? updatetime)
        {
            try
            {
                if (updatetime == null) { return BadRequest("'請檢查提交參數"); }
                DateTime updateTime = (DateTime)updatetime;
                if (ApplyLogId ==0 ||progerss == "待審核" || string.IsNullOrEmpty(progerss) || updateTime < DateTime.Now.AddMinutes(-2))
            {
                return BadRequest("'請檢查提交參數");
            }
            var dbApplyLog = _context.TTeacherApplyLogs.FirstOrDefault(a => a.FApplyLogId == ApplyLogId);
            if (dbApplyLog == null) { return StatusCode(500, "資料庫系統異常"); }
            if ("審核未通過審核通過".Contains(dbApplyLog.FProgressStatus)) { return BadRequest($"{dbApplyLog.FProgressStatus} 無法編輯狀態") ; }
            dbApplyLog.FProgressStatus = progerss;
            dbApplyLog.FNote = note;
            dbApplyLog.FReviewDatetime = updateTime;
            _context.TTeacherApplyLogs.Update(dbApplyLog);
            _context.SaveChanges();
            return Ok();
            }catch (Exception ex)
            {
                return StatusCode(500, "系統異常："+ex);
            }
        }
    }
}
