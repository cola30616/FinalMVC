using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.ViewModel;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FinalGroupMVCPrj.Controllers
{
    [AllowAnonymous]
    public class LessonController : UserInfoController
    {
        private readonly LifeShareLearnContext _context;
        public LessonController(LifeShareLearnContext context)
        {
            _context = context;
        }
        //■ ==========================     子謙作業區      ==========================■
        // GET: {baseUrl}/Lesson/Index
        //動作簡述：回傳所有課程的頁面
        public async Task<IActionResult> Index()
        {
            var fields = await _context.TCourseFields.Select(u => u.FFieldName).ToListAsync();
            var courseList = await _context.TLessonCourses
            .Include(course => course.FTeacher) // 加載 Teacher 導航屬性
            .Select(course => new LessonCourseVM
            {
                lessonCourse = course,
                // 老師名稱
                teacherName = _context.TTeachers
                    .Where(teacher => teacher.FTeacherId == course.FTeacherId)
                    .Select(teacher => teacher.FTeacherName)
                    .FirstOrDefault() ?? "找不到當前老師",
                // 科目名稱
                subjectName = _context.TCourseSubjects
                    .Where(sub => sub.FSubjectId == course.FSubjectId)
                    .Select(sub => sub.FSubjectName)
                    .FirstOrDefault() ?? "找不到科目名稱",
                // 圖片數據
                imageData = course.FPhoto,
                // 新增老師
                teacher = course.FTeacher, // 將加載的 Teacher 導航屬性賦值給 ViewModel 的 teacher 屬性
                fields = fields,
                fieldNumber = course.FSubject.FFieldId,
            })
            .ToListAsync();
            return View(courseList);
        }

        // Get: {baseUrl}/Lesson/Search?={searchText}
        [HttpGet]
        public async Task<IActionResult> Search(string searchText)
        {           
            var searchResults = await _context.TLessonCourses
                                          .Where(c => c.FName.Contains(searchText))
                                          .ToListAsync();
            return Json(searchResults);
        }


        //■ ==========================     翊妏 作業區      ==========================■
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            
            var detail = await _context.TLessonCourses
            .Include(order => order.TOrderDetails)
                .ThenInclude(evaluation => evaluation.TLessonEvaluations)
            .Where(eval => eval.FLessonCourseId == id)
            .Select(querystring => new LessonDetailViewModel
            {
                //主頁部分
                FTeacher = querystring.FTeacher,
                FName = querystring.FName,
                
                FFiled = _context.TCourseFields.Where(x=>x.FFieldId==querystring.FSubject.FFieldId).Select(x=>x.FFieldName).FirstOrDefault(),
                FSubject = querystring.FSubject,
                FVenueType = querystring.FVenueType,
                FRegPeople = _context.TOrderDetails.Where(x => x.FLessonCourseId == id).Count(),
                FPrice = querystring.FPrice,
                FTime = ((querystring.FEndTime.Value.TotalHours - querystring.FStartTime.Value.TotalHours)).ToString(),
                FMaxPeople = querystring.FMaxPeople,
                FMinPeople = querystring.FMinPeople,
                FVenueName = querystring.FVenueName,
                FCity = _context.TCities.Where(c => c.TCityDistricts.Any(d => d.FDistrictId == querystring.FDistrictId)).Select(x => x.FCityName).FirstOrDefault(),
                FDistrict = _context.TCityDistricts.Where(x => x.FDistrictId == querystring.FDistrictId).Select(x => x.FDistrictName).FirstOrDefault(),
                FAddressDetail = querystring.FAddressDetail,
                
                FOnlineLink = querystring.FOnlineLink,
                FLessonDate = querystring.FLessonDate,
                FStartTime = querystring.FStartTime,
                FEndTime = querystring.FEndTime,
                FRegDeadline = querystring.FRegDeadline,
                FDescription = querystring.FDescription,
                FEditorDes = querystring.FEditorDes,
                FRequirement = querystring.FRequirement,

                // 評價部分
                FAvgScore = querystring.TOrderDetails.SelectMany(order => order.TLessonEvaluations).Any()? 
                Math.Round(querystring.TOrderDetails.SelectMany(order => order.TLessonEvaluations).Average(evaluation => evaluation.FScore), 1) :0
            }).ToListAsync();

            return View("LDetails", detail[0]);
        }

        [HttpGet]
        public async Task<FileResult> showPicture(int id)
        {
            TLessonCourse? c = await _context.TLessonCourses.FindAsync(id);
            byte[]? Content = c?.FPhoto;
            return File(Content, "image/jpeg");
        }
    }
}
