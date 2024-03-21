using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.DTO;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Drawing.Printing;


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
            // 取得課程領域科目資訊
            var fieldWithSubjects = _context.TCourseSubjects
             .GroupBy(subject => subject.FFieldId) // 根據 FieldId 對 Subject 進行分組
             .Select(group => new CourseSubjectData
             {
                 FieldId = group.Key, // 取得分組的 FieldId
                 FieldName = _context.TCourseFields.FirstOrDefault(field => field.FFieldId == group.Key) != null ?
                            _context.TCourseFields.First(field => field.FFieldId == group.Key).FFieldName :
                            null, // 如果結果不為 null，則取得相應的 FieldName，否則設為 null
                 SubjectNames = group.Select(subject => subject.FSubjectName).ToList() // 取得每個分組的 SubjectName
             })
             .ToList();

            var courseList = await _context.TLessonCourses
             .Where(u => u.FStatus == "開放報名")
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
                fieldName = course.FSubject.FField.FFieldName,
                fieldNumber = course.FSubject.FFieldId,
                fieldWithSubjects = fieldWithSubjects
            })
            .ToListAsync();
            return View(courseList);
        }

        
        //API Calls

        // Get: {baseUrl}/Lesson/Search?={searchText}
        // 搜尋功能API
        [HttpGet]
        public async Task<IActionResult> Search(string searchText)
        {
            var searchResults = await _context.TLessonCourses
                                          .Where(c => c.FName.Contains(searchText) || c.FTeacher.FTeacherName.Contains(searchText))
                                          .Select(data => new
                                          {
                                              name = data.FName,
                                              id = data.FTeacherId,
                                              teacherId = data.FTeacher.FTeacherId,
                                              teacherName = data.FTeacher.FTeacherName,
                                          })
                                          .Distinct() 
                                          .ToListAsync();
            return Json(searchResults);
        }

       
        // GET: {baseUrl}/Lesson/CourseList
        //課程篩選用API
        [HttpGet]
        public async Task<IActionResult> CourseList(CourseListDTO courseListDTO)
        {
            int pageSize = courseListDTO.PageSize ?? 9;
            int skip = (courseListDTO.Page - 1) * pageSize;
            
            // 全部資料
            var query = _context.TLessonCourses.AsQueryable();
            // 只選開放報名的課
            query = query.Where(u => u.FStatus == "開放報名");
            // 關鍵字查詢
            if (!string.IsNullOrEmpty(courseListDTO.Keyword))
            {
                query = query.Where(course =>
                    course.FName.Contains(courseListDTO.Keyword) ||                   
                    course.FTeacher.FTeacherName.Contains(courseListDTO.Keyword) ||
                    course.FSubject.FSubjectName.Contains(courseListDTO.Keyword) ||
                    course.FSubject.FField.FFieldName.Contains(courseListDTO.Keyword)
                );
            }
            // 領域篩選
            if (courseListDTO.FieldId.HasValue)
            {
                query = query.Where(course => course.FSubject.FFieldId == courseListDTO.FieldId);
            }
            // 科目篩選
            if (!string.IsNullOrEmpty(courseListDTO.subjectName))
            {
                query = query.Where(course => course.FSubject.FSubjectName == courseListDTO.subjectName);
            }
            // 最低價格篩選
            if (courseListDTO.MinPrice.HasValue)
            {
                query = query.Where(course => course.FPrice >= courseListDTO.MinPrice);
            }
            // 最高價格篩選
            if (courseListDTO.MaxPrice.HasValue)
            {
                query = query.Where(course => course.FPrice <= courseListDTO.MaxPrice);
            }
            // 評價
            //if (courseListDTO.MinRating.HasValue)
            //{
            //    query = query.Where(course => course.TOrderDetails.Where(s => s.TLessonEvaluations.SelectMany(u => u.FScore)) >= courseListDTO.MinRating);
            //}

            //if (courseListDTO.MaxRating.HasValue)
            //{
            //    query = query.Where(course => course.FRating <= courseListDTO.MaxRating);
            //}

            // 
            if (!string.IsNullOrEmpty(courseListDTO.SortBy))
            {
                switch (courseListDTO.SortType)
                {                  
                    case "newest":
                        query = query.OrderByDescending(course => course.FLessonDate);
                        break;
                    
                    case "PriceDesc":
                        query = query.OrderByDescending(course => course.FPrice);
                        break;
                    case "PriceAsc":
                        query = query.OrderBy(course => course.FPrice);
                        break;

                }
            }
            // 目前課程總數量
            int totalCount = await query.CountAsync();

            var courseList = await query
                .Skip(skip)
                .Take(pageSize)
                .Select(course => new LessonCourseVM
                {
                    lessonCourse = course,
                    teacherName = course.FTeacher.FTeacherName,
                    subjectName = course.FSubject.FSubjectName,
                    imageData = course.FPhoto,
                    fieldName = course.FSubject.FField.FFieldName,
                    fieldNumber = course.FSubject.FFieldId
                })
                .ToListAsync();

            var response = new
            {
                totalCount,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                currentPage = courseListDTO.Page,
                pageSize,
                courses = courseList,
                
            };

            return Json(response);
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
                FTeacherId = querystring.FTeacherId,

                //// 評價部分
                FLessonCourseId = querystring.FLessonCourseId,
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

    //課程細節，viewComponent
    public class CourseItemView : ViewComponent
    {
        private readonly LifeShareLearnContext _context;

        public CourseItemView(LifeShareLearnContext context)
        {
            _context = context;
        }


        // 這邊使用viewComponent
        public async Task<IViewComponentResult> InvokeAsync(int? id)
        {
            var fields = await _context.TCourseFields.Select(u => u.FFieldName).ToListAsync();
                      
            var courseList = await _context.TLessonCourses
                .Where(u => u.FTeacherId == id && u.FStatus == "開放報名")              
                .Select(course => new LessonCourseVM
                {
                    lessonCourse = course,
                    teacherName = _context.TTeachers
                        .Where(teacher => teacher.FTeacherId == course.FTeacherId)
                        .Select(teacher => teacher.FTeacherName)
                        .FirstOrDefault() ?? "找不到當前老師",
                    subjectName = _context.TCourseSubjects
                        .Where(sub => sub.FSubjectId == course.FSubjectId)
                        .Select(sub => sub.FSubjectName)
                        .FirstOrDefault() ?? "找不到科目名稱",
                    imageData = course.FPhoto,
                    teacher = course.FTeacher,
                    fields = fields,
                    fieldName = course.FSubject.FField.FFieldName,
                    fieldNumber = course.FSubject.FFieldId,
                })               
                .ToListAsync();

            return View(courseList);
        }
    }

}
