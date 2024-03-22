using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.DTO;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
                    TeacherPlanLink = dbApplyLog.FTeacherPlanLink,
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

                if(dbApplyLog.FProgressStatus == "審核通過")
                {
                    var dbTeacher = _context.TTeachers.SingleOrDefault(t=>t.FMemberId == dbApplyLog.FMemberId);
                    if (dbTeacher != null) { return BadRequest("不當操作，該會員已有老師資格"); }
                    dbTeacher = new TTeacher
                    {
                        FMemberId = dbApplyLog.FMemberId,
                        FTeacherName = dbApplyLog.FTeacherName,
                        FContactInfo = dbApplyLog.FContactInfo,
                        FJoinDatetime = DateTime.Now,
                        FIntroduction = dbApplyLog.FIntroduction,
                    };
                    _context.TTeachers.Add(dbTeacher);
                    var dbMember = _context.TMembers.SingleOrDefault(m =>m.FMemberId == dbApplyLog.FMemberId);
                    if (dbMember == null) { return StatusCode(500, "系統異常"); }
                    dbMember.FQualifiedTeacher = true;
                    _context.TMembers.Update(dbMember);
                }
                _context.SaveChanges();
                return Ok();
            }catch (Exception ex)
            {
                return StatusCode(500, "系統異常："+ex);
            }
        }
        //■ ==========================     東霖作業區      ==========================■
        public IActionResult CheckList()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> editTeacherSubject(string result,int teacherid)
        {
            // 刪除 老師id = teacherid 的多筆科目
            var deleteTeacherSubject = await _context.TTeacherSubjects.Where(id=>id.FTeacherId == teacherid).ToListAsync();
            if (deleteTeacherSubject != null) { _context.TTeacherSubjects.RemoveRange(deleteTeacherSubject); }
            await _context.SaveChangesAsync();

            if (result != null)
            {
                List<int> subjectIds = result.Split(',').Select(Int32.Parse).ToList();
                foreach (var subjectId in subjectIds)
                {
                    var newTeacherSubject = new TTeacherSubject
                    {
                        FTeacherId = teacherid,
                        FSubjectId = subjectId
                    };

                    _context.TTeacherSubjects.Add(newTeacherSubject);

                }
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
        //動作簡述 : 作為dataTable套件的tbody
        public IActionResult CheckListDataJson()
        {
            var tCheckList = _context.TTeachers
                .Include(t=>t.FMember)
                .Include(t => t.TTeacherSubjects)
                .ThenInclude(t => t.FSubject)
                .AsQueryable() ;
            IEnumerable<AdminTCheckVM> tCheckVMCollection =
                new List<AdminTCheckVM>(tCheckList.Select(a => new AdminTCheckVM
                {
                    TeacherId = a.FTeacherId,
                    //SubjectName = a.TTeacherSubjects.Select(ts => ts.FSubject.FSubjectName),
                    SubjectName = string.Join("、", a.TTeacherSubjects.Select(ts => ts.FSubject.FSubjectName)),
                    TeacherProfilePic = a.FTeacherProfilePic,
                    TeacherName = a.FTeacherName,
                    //Email = a.FMember.FEmail,
                    RealName = a.FMember.FRealName,
                    Note = a.FNote ?? "-",
                }
                ));
            return Json(new { data = tCheckVMCollection });
        }
        ////讀取老師可開課科目
        //public IActionResult EditPartialViewInfo(int TeacherId)
        //{
        //    var a = _context.TCourseSubjects
        //        .Include(a => a.TTeacherSubjects)
        //        .ThenInclude(a=>a.FTeacher)
        //        .ThenInclude(a=>a.FMember)
        //        .Where(a => a.TTeacherSubjects.Any(ts => ts.FTeacherId == TeacherId))
        //    .Select(a => new
        //    {
        //        SubjectName = a.FSubjectName,
        //        Email = a.TTeacherSubjects
        //                .FirstOrDefault(ts => ts.FTeacherId == TeacherId)
        //                .FTeacher.FMember.FEmail,
        //        TeacherName = a.TTeacherSubjects
        //                .FirstOrDefault(ts => ts.FTeacherId == TeacherId)
        //                .FTeacher.FTeacherName,
        //    });
        //    return Json(a);
        //}
        //讀取老師可開課科目
        public IActionResult EditPartialViewInfo(int TeacherId)
        {
            var a = _context.TCourseSubjects
                .Include(a => a.TTeacherSubjects)
                .ThenInclude(a => a.FTeacher)
                .ThenInclude(a => a.FMember)
                .Where(a => a.TTeacherSubjects.Any(ts => ts.FTeacherId == TeacherId))
            .Select(a =>  new List<string> { { a.FSubjectId.ToString() }, { a.FSubjectName } }
            );
            return Json(a);
        }
        //讀取領域
        public IActionResult Fields()
        {
            var fields = _context.TCourseFields.Select(a => a.FFieldName).Distinct();
            return Json(fields);
        }

        //根據領域名稱讀取科目和科目id
        public IActionResult Subjects(string fieldName)
        {
            var subjects = _context.TCourseSubjects.Where(a => a.FField.FFieldName == fieldName).Select(a => new List<string> { { a.FSubjectId.ToString() }, { a.FSubjectName } }).Distinct();
            return Json(subjects);
        }
        //根據科目名稱讀取科目id
        public IActionResult SubjectID(string subjectName)
        {
            var subjectid = _context.TCourseSubjects
                .Where(s => s.FSubjectName == subjectName)
                .Select(id => id.FSubjectId.ToString());
            return Json(subjectid);
        }
        //根據老師id讀取老師名稱和電子郵件
        public IActionResult trNameEmail(int TeacherId)
        {
            var a = _context.TTeachers
                .Include(a => a.FMember)
                .Where(a => a.FTeacherId == TeacherId)
            .Select(a => new
            {
                Email = a.FMember.FEmail,
                TeacherName = a.FTeacherName
            });
            return Json(a);
        }

    }
}
