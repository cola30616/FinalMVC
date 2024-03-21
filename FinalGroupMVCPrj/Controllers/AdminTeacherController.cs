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
        //■ ==========================     東霖作業區      ==========================■
        public IActionResult CheckList()
        {
            return View();
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
                .Select(id => id.FSubjectId);
            return Json(subjectid);
        }
    }
}
