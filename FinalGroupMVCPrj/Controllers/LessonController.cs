using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.DTO;
using FinalGroupMVCPrj.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                fieldName = course.FSubject.FField.FFieldName,
                fieldNumber = course.FSubject.FFieldId,
            })
            .ToListAsync();
            return View(courseList);
        }

        #region 
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
                                          .ToListAsync();
            return Json(searchResults);
        }

        //■ ==========================     子謙作業區      ==========================■
        // GET: {baseUrl}/Lesson/CourseList
        //課程篩選用API
        [HttpGet]
        public async Task<IActionResult> CourseList(CourseListDTO courseListDTO)
        {
            int pageSize = courseListDTO.PageSize ?? 9;
            int skip = (courseListDTO.Page - 1) * pageSize;
            // 取得課程領域科目資訊
            var fieldWithSubjects = _context.TCourseSubjects
             .GroupBy(subject => subject.FFieldId) // 根據 FieldId 對 Subject 進行分組
             .Select(group => new
             {
                 FieldId = group.Key, // 取得分組的 FieldId
                 FieldName = _context.TCourseFields.FirstOrDefault(field => field.FFieldId == group.Key) != null ?
                            _context.TCourseFields.First(field => field.FFieldId == group.Key).FFieldName :
                            null, // 如果結果不為 null，則取得相應的 FieldName，否則設為 null
                 SubjectNames = group.Select(subject => subject.FSubjectName).ToList() // 取得每個分組的 SubjectName
             })
             .ToList();
            // 全部資料
            var query = _context.TLessonCourses.AsQueryable();
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

            if (courseListDTO.FieldId.HasValue)
            {
                query = query.Where(course => course.FSubject.FFieldId == courseListDTO.FieldId);
            }

            if (courseListDTO.SubjectId.HasValue)
            {
                query = query.Where(course => course.FSubjectId == courseListDTO.SubjectId);
            }

            if (courseListDTO.MinPrice.HasValue)
            {
                query = query.Where(course => course.FPrice >= courseListDTO.MinPrice);
            }

            if (courseListDTO.MaxPrice.HasValue)
            {
                query = query.Where(course => course.FPrice <= courseListDTO.MaxPrice);
            }

            //if (courseListDTO.MinRating.HasValue)
            //{
            //    query = query.Where(course => course.FSC >= courseListDTO.MinRating);
            //}

            //if (courseListDTO.MaxRating.HasValue)
            //{
            //    query = query.Where(course => course.FRating <= courseListDTO.MaxRating);
            //}

            if (!string.IsNullOrEmpty(courseListDTO.SortBy))
            {
                switch (courseListDTO.SortBy)
                {                  
                    case "newest":
                        query = query.OrderByDescending(course => course.FLessonDate);
                        break;
                    //case "popular":
                    //    // 在這裡根據你的定義添加最熱門課程的排序邏輯
                    //    break;
                    case "enrollment":
                        query = query.OrderByDescending(course => course.FMaxPeople);
                        break;
                    //case "rating":
                    //    query = query.OrderByDescending(course => course.FRating);
                    //    break;
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
                fieldWithSubjects
            };

            return Json(response);
        }
        #endregion


        //■ ==========================     翊妏 作業區      ==========================■
        [HttpGet]
        public IActionResult Details()
        {
            return View("LDetails");
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
        public async Task<IViewComponentResult> InvokeAsync(int page = 1, int pageSize = 9)
        {
            var fields = await _context.TCourseFields.Select(u => u.FFieldName).ToListAsync();
            var courseList = await _context.TLessonCourses
                .Include(course => course.FTeacher)
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
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return View(courseList);
        }
    }

}
