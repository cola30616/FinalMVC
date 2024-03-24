using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.DTO;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.Pkcs;
using System.Security.Policy;
using static NuGet.Packaging.PackagingConstants;
using System.Text;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Newtonsoft.Json.Linq;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinalGroupMVCPrj.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherAdminController : UserInfoController
    {
        private readonly HttpClient _httpClient;
        private IWebHostEnvironment _hostEnv;
        private readonly LifeShareLearnContext _context;
        public TeacherAdminController(LifeShareLearnContext context, IWebHostEnvironment env)
        {
            _context = context;
            _hostEnv = env;
            _httpClient = new HttpClient();
        }
        //■ ==========================     翊妏作業區      ==========================■

        // GET: TeacherAdmin/LessonList
        //動作簡述：回傳老師課程清單資訊
        [HttpGet]
        public IActionResult LessonList()
        {
            return View();
        }
        // GET: TeacherAdmin/LessonList
        //動作簡述：回傳老師課程清單資訊
        [HttpGet]
        public IActionResult ListDataJson()
        {
            var lessons = _context.TLessonCourses.AsQueryable().Where(x => x.FTeacherId == GetCurrentTeacherId()).Select(querystring => new LessonListViewModel
            {
                Code = querystring.FCode,
                Name = querystring.FName,
                Filed = _context.TCourseFields.Where(x => x.FFieldId == querystring.FSubject.FFieldId).Select(x => x.FFieldName).FirstOrDefault(),
                Price = (int)querystring.FPrice,
                LessonDate = querystring.FLessonDate,
                Time = ((querystring.FEndTime.Value.TotalHours == null ? 0 : querystring.FEndTime.Value.TotalHours) - (querystring.FStartTime.Value.TotalHours == null ? 0 : querystring.FStartTime.Value.TotalHours)).ToString() + "hr",
                MaxPeople = querystring.FMaxPeople,
                RegPeople = _context.TOrderDetails.Where(x => x.FLessonCourseId == querystring.FLessonCourseId).Count(),
                Status = querystring.FStatus,
                VenueType = querystring.FVenueType == true ? "實體" : "線上",
                lessonid = querystring.FLessonCourseId
            });
            return Json(new { data = lessons });
        }
        [HttpGet]
        public IActionResult LessonOpen()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LessonCreate(string? code)
        {
            //歷史開課
            if (code != null)
            {
                LessonCreateViewModel lesson = new LessonCreateViewModel();
                var course = _context.TLessonCourses.Where(x => x.FCode == code).FirstOrDefault();
                if (course != null)
                {
                    lesson.FLessonCourseId = course.FLessonCourseId;
                    lesson.FSubjectId = course.FSubjectId;
                    lesson.FSubject = _context.TCourseSubjects.Where(x => x.FSubjectId == lesson.FSubjectId).Select(x => x.FSubjectName).FirstOrDefault();
                    lesson.FFiled = (from s in _context.TCourseSubjects
                                     join f in _context.TCourseFields on s.FFieldId equals f.FFieldId
                                     where s.FSubjectId == course.FSubjectId
                                     select f.FFieldName).Distinct().FirstOrDefault();
                    lesson.FName = course.FName;
                    lesson.FCode = course.FCode;
                    lesson.FTeacherId = GetCurrentTeacherId();

                    //lesson.FPhoto = course.FPhoto==null?null:ConvertByteArrayToIFormFile(course.FPhoto,course.FName);
                    ViewBag.courseType = "old";

                    return View("LCreate", lesson);
                }
            }

            ViewBag.courseType = "new";
            //直接開課
            return View("LCreate", new LessonCreateViewModel());
        }
        // POST: TeacherAdmin/LessonCreate
        [RequestFormLimits(MultipartBodyLengthLimit = 10240000)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LessonCreate(LessonCreateViewModel lesson, string status, string getcourseType)
        {
            int teacherid = GetCurrentTeacherId();
            string code = "";
            try
            {
                TLessonCourse course = new TLessonCourse();
                if (getcourseType == "new")
                {
                    course.FName = lesson.FName;
                    course.FSubjectId = lesson.FSubjectId;
                    course.FSubjectId = Convert.ToInt32(lesson.FSubject);
                    code = loadCode(Convert.ToInt32(lesson.FFiled), course.FSubjectId);
                }
                else if (getcourseType == "old")
                {
                    course.FSubjectId = _context.TCourseSubjects.Where(x => x.FSubjectName == lesson.FSubject).Select(x => x.FSubjectId).FirstOrDefault();
                    course.FName = lesson.FName;

                }

                course.FVenueName = lesson.FVenueName;
                course.FTeacherId = teacherid;
                course.FEditorDes = lesson.FEditorDes;
                var count = _context.TLessonCourses.Where(x => x.FSubjectId == Convert.ToInt32(course.FSubjectId)).Select(x=>x.FCode).Distinct().Count();
                course.FCode = lesson.FCode != null ? lesson.FCode : $"{code}{(count + 1):D3}";
                course.FDescription = lesson.FDescription;
                course.FRequirement = lesson.FRequirement;
                course.FPrice = lesson.FPrice;
                course.FHomeworkDescription = lesson.FHomeworkDescription;
                course.FMaxPeople = lesson.FMaxPeople;
                course.FMinPeople = lesson.FMinPeople;
                course.FLessonDate = lesson.FLessonDate;
                course.FRegDeadline = lesson.FRegDeadline;
                course.FStartTime = lesson.FStartTime;
                course.FEndTime = lesson.FEndTime;
                course.FVenueName = lesson.FVenueName;
                course.FDistrictId = lesson.FDistrictId;
                course.FAddressDetail = lesson.FAddressDetail;
                course.FVenueType = lesson.FVenueType;
                //會議室暫定用固定連結
                course.FOnlineLink = lesson.FVenueType == false ? "https://meet.google.com/tek-pkav-obh" : null;

                course.FStatus = checkStatus(status);
                // course.FPhoto = lesson.FPhoto;
                if (lesson.FPhoto != null)
                {

                    course.FPhoto = await ReadUploadImage(lesson.FPhoto);
                }
                _context.Add(course);
                await _context.SaveChangesAsync();
                TempData["Success"] = "開課成功";
            }
            catch (Exception e)
            {

                TempData["Error"] = "開課失敗";
            }
            return RedirectToAction(nameof(LessonList));

        }

        // GET: TeacherAdmin/LessonEdit/10000
        [HttpGet]
        public async Task<IActionResult> LessonEdit(int? id)
        {
            if (id == null || _context.TLessonCourses == null)
            {
                return NotFound();
            }

            var course = await _context.TLessonCourses.FindAsync(id);
            LessonCreateViewModel lesson = new LessonCreateViewModel();
            if (course == null)
            {
                return NotFound();
            }
            var selectcity= (from s in _context.TCities
              join f in _context.TCityDistricts on s.FCityId equals f.FCityId
             where f.FDistrictId== course.FDistrictId
              select s.FCityId).Distinct().FirstOrDefault();
            var CitySelectList = LessonCreateViewModel.GetCitySelectList();
            foreach (var s in CitySelectList)
            {
                if (s.Value == selectcity.ToString())
                {
                    s.Selected = true;
                    break;
                }
            }

            ViewBag.CitySelectList = CitySelectList;
            ViewBag.DistrictSelect = course.FDistrictId;
            if (course != null)
            {
                lesson.FLessonCourseId = course.FLessonCourseId;
                lesson.FSubjectId = course.FSubjectId;
                lesson.FSubject = _context.TCourseSubjects.Where(x => x.FSubjectId == lesson.FSubjectId).Select(x => x.FSubjectName).FirstOrDefault();
                lesson.FFiled = (from s in _context.TCourseSubjects
                                 join f in _context.TCourseFields on s.FFieldId equals f.FFieldId
                                 where s.FSubjectId == course.FSubjectId
                                 select f.FFieldName).Distinct().FirstOrDefault();
                lesson.FName = course.FName;
                lesson.FCode = course.FCode;
                lesson.FTeacherId = GetCurrentTeacherId();
                lesson.FRequirement = course.FRequirement;
                lesson.FRegDeadline = course.FRegDeadline;
                lesson.FEditorDes = course.FEditorDes;
                lesson.FHomeworkDescription = course.FHomeworkDescription;
                lesson.FPrice = course.FPrice;
                lesson.FMinPeople = course.FMinPeople;
                lesson.FMaxPeople = course.FMaxPeople;
                lesson.FStartTime = course.FStartTime;
                lesson.FEndTime = course.FEndTime;
                lesson.FVenueType = course.FVenueType;
                lesson.FVenueName = course.FVenueName;
                lesson.FDistrictId = course.FDistrictId;
                lesson.FAddressDetail = course.FAddressDetail;
                lesson.FLessonDate = course.FLessonDate;
                lesson.FStatus = course.FStatus;
                lesson.FStatusNote = course.FStatusNote;
                lesson.FOnlineLink = course.FOnlineLink;
                lesson.FDistrictId = course.FDistrictId;
                lesson.FPhoto = course.FPhoto == null ? null : ConvertByteArrayToIFormFile(course.FPhoto, course.FName);

            }
            return View("LEdit", lesson);

        }
        // POST: TeacherAdmin/LessonEdit/10000
        [RequestFormLimits(MultipartBodyLengthLimit = 10240000)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LessonEdit(int? id, LessonCreateViewModel lesson,string? openstatus)
        {
            try
            {
                if (id != lesson.FLessonCourseId)
                {
                    return NotFound();
                }
                var course = await _context.TLessonCourses.FindAsync(id);
                if (course != null)
                {
                    course.FLessonCourseId = lesson.FLessonCourseId;
                    course.FVenueName = lesson.FVenueName;
                    course.FTeacherId = GetCurrentTeacherId();
                    //course.FSubjectId = Convert.ToInt32(lesson.FSubject);
                    course.FSubjectId = _context.TCourseSubjects.Where(x => x.FSubjectName == lesson.FSubject).Select(x => x.FSubjectId).FirstOrDefault();
                    course.FName = lesson.FName;
                    course.FDescription = lesson.FDescription;
                    course.FRequirement = lesson.FRequirement;
                    course.FPrice = lesson.FPrice;
                    course.FHomeworkDescription = lesson.FHomeworkDescription;
                    course.FMaxPeople = lesson.FMaxPeople;
                    course.FMinPeople = lesson.FMinPeople;
                    course.FLessonDate = lesson.FLessonDate;
                    course.FRegDeadline = lesson.FRegDeadline;
                    course.FStartTime = lesson.FStartTime;
                    course.FEndTime = lesson.FEndTime;
                    course.FVenueName = lesson.FVenueName;
                    course.FDistrictId = lesson.FDistrictId;
                    course.FAddressDetail = lesson.FAddressDetail;
                    course.FVenueType = lesson.FVenueType;
                    if (lesson.FVenueType == false)
                    {
                        course.FOnlineLink = "https://meet.google.com/tek-pkav-obh";
                    }


                    if (lesson.FPhoto != null)
                    {
                        course.FPhoto = await ReadUploadImage(lesson.FPhoto);
                    }
                   
           
                   course.FStatus = openstatus == "TempSave"? "未開放" : "開放報名";
                  
                   
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "成功儲存變更";

                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "儲存變更失敗";
                return View("LEdit");
            }
            return RedirectToAction(nameof(LessonList));

        }
        [HttpPost]  
        public async Task<IActionResult> LessonCancel(int? id, [FromBody] CancelLessonData cancelData)
        {
            if (id == null || cancelData.Reason == null)
            {
                return BadRequest(); // 如果 id 或 data 为 null，则返回 400 错误
            }
            var course = await _context.TLessonCourses.FindAsync(id );
            if (course == null)
            {
                return NotFound(); // 如果未找到课程，则返回 404 错误
            }
            try
            {
                // 更新课程状态和状态说明
                course.FStatus = "課程取消";
                course.FStatusNote = cancelData.Reason.ToString();

                _context.Update(course);
                await _context.SaveChangesAsync();
                //sweetalert有要重新整理才會出現
              
                return Ok(course); // 返回 200 状态码表示操作成功
            }
            catch (Exception ex)
            {
                // 处理异常情况，比如数据库更新失败
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpGet]
        public  IActionResult LessonTestPhoto()
        {

            return View();

        }

      

        public IFormFile ConvertByteArrayToIFormFile(byte[] fileBytes, string fileName)
        {
            // 创建一个内存流，并将 byte[] 写入其中
            using (MemoryStream memoryStream = new MemoryStream(fileBytes))
            {
                // 使用 FormFile 类创建一个模拟的 IFormFile 对象
                var formFile = new FormFile(memoryStream, 0, fileBytes.Length, "FPhoto", "II.png")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/png", // 根据需要设置 ContentType
                    ContentDisposition = "form-data; name=\"FPhoto\";filename=\"II.png\""
                };

                return formFile;
            }
        }
        public string checkStatus(string status)
        {
            string detail = "";

            switch (status)
            {
                case "TempSave":
                    detail = "未開放";
                    break;
                case "Create":
                    detail = "開放報名";
                    break;
                case "cancel":
                    detail = "課程取消";
                    break;
                default:
                    detail = "未知狀態";
                    break;
            }
            //1.課程取消
            // 2.未開放
            // 3.開放報名
            // 4.報名截止
            // 5.課程結束
            return detail;
        }

        [HttpGet]
        public IActionResult TeacherAllowFiled()
        {

            int teacherid = GetCurrentTeacherId();
            //帶目前老師可開的領域和科目
            //老師可開的科目
            //var allowsubject = _context.TTeacherSubjects.Where(x => x.FTeacherId == teacherid).Select(x => x.FSubjectId).ToList();
            //var subjectTofiled = _context.TCourseSubjects.Where(x => x.FSubjectId==(allowsubject)).Select(x => x.FFieldId).ToList();
            //var allowFiled = _context.TCourseFields.Where(x=>x.FFieldId.Equals( subjectTofiled)).Distinct().Select(x=>new { x.FFieldName ,x.FFieldCode}).ToListAsync();
            var allowFiled = (
                                            from ts in _context.TTeacherSubjects
                                            join cs in _context.TCourseSubjects on ts.FSubjectId equals cs.FSubjectId
                                            join cf in _context.TCourseFields on cs.FFieldId equals cf.FFieldId
                                            where ts.FTeacherId == teacherid
                                            select new { cf.FFieldName, cf.FFieldId }
                                        ).Distinct().ToList();
            return Json(allowFiled);
        }

        [HttpGet]
        public IActionResult TeacherAllowSubject(int filedId)
        {
            int teacherid = GetCurrentTeacherId();
            var allowSubject = (
                                            from ts in _context.TTeacherSubjects
                                            join cs in _context.TCourseSubjects on ts.FSubjectId equals cs.FSubjectId
                                            join cf in _context.TCourseFields on cs.FFieldId equals cf.FFieldId
                                            where ts.FTeacherId == teacherid && cf.FFieldId == filedId
                                            select new { cs.FSubjectId, cs.FSubjectName }
                                        ).Distinct().ToList();
            return Json(allowSubject);
        }
        [HttpGet]
        public IActionResult allCity()
        {
            var city = _context.TCities.Select(x => new { x.FCityId, x.FCityName }).Distinct();
            return Json(city);
        }

        [HttpGet]
        public IActionResult allDistrict(int cityid)
        {
            var district = _context.TCityDistricts.Where(x => x.FCityId == cityid).Select(x => new { x.FDistrictId, x.FDistrictName });
            return Json(district);
        }


        public string loadCode(int? filedid, int? subjectid)
        {
            var code1 = _context.TCourseFields.Where(x => x.FFieldId == filedid).Select(x => x.FFieldCode).FirstOrDefault();
            var code2 = _context.TCourseSubjects.Where(x => x.FSubjectId == subjectid).Select(x => x.FSubjectCode).FirstOrDefault();
            string code = code1 + code2;

            return code;
        }
        public async Task<FileResult> showPicture(int id)
        {
            TLessonCourse? c = await _context.TLessonCourses.FindAsync(id);
            byte[]? content = c?.FPhoto;
            return File(content, "image/*");
        }
        private async Task<byte[]> ReadUploadImage(IFormFile photo)
        {
            if (photo != null && photo.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await photo.CopyToAsync(memoryStream);
                    byte[] fileBytes = memoryStream.ToArray();
                    return fileBytes;
                    // 在这里可以将 fileBytes 保存到数据库或者进行其他操作
                }
            }
            else
            {
                return null;
            }
            //if (Request.Form.Files["FPhoto"] != null)
            //{
            //    using自動資源管理
            //    using (BinaryReader br = new BinaryReader(
            //        Request.Form.Files["FPhoto"].OpenReadStream()))
            //    {
            //        photo = br.ReadBytes((int)Request.Form.Files["FPhoto"].Length);
            //    }
            //}
            //else
            //{
            //    //避免原本有圖的被覆蓋成預設沒圖
            //   TLessonCourse c = await _context.TLessonCourses.FindAsync(photo);
            //    photo.fphoto = c.fphoto;
            //    //解除c的追蹤
            //    _context.entry(c).state = entitystate.detached;
            //}
        }


        [HttpGet]
        public IActionResult TeacherOpenedFiled()
        {

            int teacherid = GetCurrentTeacherId();
            //帶目前老師開過的課

            var openedFields = (
                            from cs in _context.TCourseSubjects
                            join cf in _context.TCourseFields on cs.FFieldId equals cf.FFieldId
                            join lc in _context.TLessonCourses.Where(x => x.FTeacherId == teacherid) on cs.FSubjectId equals lc.FSubjectId
                            select new { cf.FFieldName, cf.FFieldId }
                        ).Distinct().ToList();

            return Json(openedFields);
        }

        [HttpGet]
        public IActionResult TeacherOpenedSubject(int filedId = 1)
        {
            int teacherId = GetCurrentTeacherId();

            var openedSubjects = (
                from s in _context.TCourseSubjects
                join l in _context.TLessonCourses.Where(x => x.FTeacherId == teacherId) on s.FSubjectId equals l.FSubjectId
                join cf in _context.TCourseFields.Where(x => x.FFieldId == filedId) on s.FFieldId equals cf.FFieldId
                select new { s.FSubjectName, s.FSubjectId }
            ).ToList();

            return Json(openedSubjects);
        }

        [HttpGet]
        public IActionResult historycourse()
        {
            int teacherId = GetCurrentTeacherId();
            var history = _context.TLessonCourses
                  .Where(x => x.FTeacherId == teacherId)
                  .Select(x => new
                  {
                      x.FCode,
                      x.FName,
                  })
                  .GroupBy(x => x.FCode) // 根据 FCode 进行分组
                  .Select(group => group.First()) // 选择每个分组中的第一个元素，确保 FCode 是唯一的
                  .ToList();

            return Json(history);
        }

        //■ ==========================     東霖作業區      ==========================■
        protected int GetCurrentMemberId()
        {
            var idText = HttpContext.User.Claims.Where(u => u.Type == "MemberId").FirstOrDefault();
            if (idText != null)
            {
                return Convert.ToInt32(idText.Value);
            }
            return 0;
        }
        protected int GetCurrentTeacherId()
        {
            int currentMemberId = GetCurrentMemberId();
            var dbMember = _context.TMembers.SingleOrDefault(m => m.FMemberId == currentMemberId);
            if (dbMember != null && dbMember.FQualifiedTeacher)
            {
                int teacherId = 0;
                var dbTeacher = _context.TTeachers.SingleOrDefault(t => t.FMemberId == currentMemberId);
                if (dbTeacher != null)
                {
                    teacherId = dbTeacher.FTeacherId;
                    return teacherId;
                }
            }
            return 0;
        }
        //方法簡述：將二進位資料轉URL
        private static string GetImageDataURL(byte[] image)
        {
            string blobDataURL = "";
            if (image != null)
            {
                string base64String = Convert.ToBase64String(image);

                blobDataURL = $"data:image/jpeg;base64,{base64String}";
                return blobDataURL;
            }
            return null;
        }
        // GET: TeacherAdmin/TBasicInfo
        //動作簡述：回傳老師基本資訊
        [HttpGet]
        public IActionResult TBasicInfo()
        {
            int id = GetCurrentTeacherId();
            IEnumerable<TeacherBasicViewModel> vBasicVMCollection = new List<TeacherBasicViewModel>(
                    _context.TTeachers
                      .Where(t => t.FTeacherId == id)
                    .Select(t => new TeacherBasicViewModel
                    {
                        TeacherId = id,
                        TeacherName = t.FTeacherName,
                        TeacherProfilePicURL = (t.FTeacherProfilePic != null) ? GetImageDataURL(t.FTeacherProfilePic) : "https://i.imgur.com/xcZh1PZ.png",
                        Introduction = t.FIntroduction,
                        ContactInfo = t.FContactInfo,
                        Note = t.FNote,
                        SubjectName = t.TTeacherSubjects.Select(ts => ts.FSubject.FSubjectName),
                        TeacherModel = t
                    })
                );
            return View("TBasicinfo", vBasicVMCollection);
        }
        //
        //動作簡述 : 上傳老師頭像(將老師頭像編輯成另一張照片)
        [HttpPost]
        public async Task<IActionResult> TEditTrProfilePic(TTeacher teacher, IFormFile ProfilePic)
        {
            TTeacher? dbTeacher = _context.TTeachers
                .Where(t => t.FMemberId == GetCurrentMemberId()).FirstOrDefault();
            if (dbTeacher == null) { return BadRequest("系統異常"); }
            if (ProfilePic != null && ProfilePic.Length > 0)
            {
                using (BinaryReader br = new BinaryReader(ProfilePic.OpenReadStream()))
                {
                    dbTeacher.FTeacherProfilePic = br.ReadBytes((int)ProfilePic.Length);
                }
            }
            else
            {
                //這裡絕對不會進來，回從前端處理
                //return
                //ModelState.IsValid = false ，參數(ProfilePic)=null 會有錯 errormessage: ProfilePic是必要項
                HttpResponseMessage response =await _httpClient.GetAsync("https://img.onl/eZD9Pm");
                response.EnsureSuccessStatusCode();
                // 读取图片数据并转换为字节数组
                byte[] imageBytes =await response.Content.ReadAsByteArrayAsync();
                dbTeacher.FTeacherProfilePic = imageBytes;
            } 
            //dbTeacher.FTeacherName = teacher.FTeacherName; 
            teacher = dbTeacher as TTeacher;
            foreach (var modelStateEntry in ModelState.Values)
            {
                foreach (var error in modelStateEntry.Errors)
                {
                    var errorMessage = error.ErrorMessage;
                    Console.WriteLine(errorMessage);
                }
            }
            if (ModelState.IsValid)
            {
                _context.Update(teacher);
                await _context.SaveChangesAsync();
                //RedirectToAction沒有反應，所以在script導頁
                return RedirectToAction("TBasicInfo");
            }
            return Content("修改失敗");
        }
        // POST: TeacherAdmin/TEdittrbasicprofile
        //動作簡述：編輯老師基本資料(不包含老師頭像)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TEdittrbasicprofile(TTeacher teacher)
        {
            // https://www.google.com/search?q=.net+core+mva+update+partial+model+property&rlz=1C1ONGR_zh-TWTW1027TW1027&oq=.net+core+mva+update+partial+model+property&gs_lcrp=EgZjaHJvbWUyBggAEEUYOdIBCTMwMzYzajBqMagCALACAA&sourceid=chrome&ie=UTF-8#ip=1
            // ======== 局部更新資料 start =========
            TTeacher? dbTeacher = _context.TTeachers.Where(t => t.FTeacherId == GetCurrentTeacherId()).FirstOrDefault();
            if (dbTeacher == null)
            {
                return BadRequest("系統異常");
            }
            dbTeacher.FTeacherName = teacher.FTeacherName;
            dbTeacher.FIntroduction = teacher.FIntroduction;
            dbTeacher.FContactInfo = teacher.FContactInfo;
            dbTeacher.FNote = teacher.FNote;
            teacher = dbTeacher as TTeacher;
            // ======== 局部更新資料 end =========
            if (ModelState.IsValid)
            {
                _context.Update(teacher);
                await _context.SaveChangesAsync();
                //重導到首頁
                return RedirectToAction("TBasicInfo");
            }
            return View(teacher);
        }
        /////////////////////////////////////// ///////////////////////////////////////
        // POST: TeacherAdmin/TAddtrimage
        //動作簡述：新增單/多張圖片
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> TAddtrimage(Models.TTeacherImage trimg, IList<IFormFile> f)
        {
            //byte[] fileBytes = await ConvertFileToByteArrayAsync(file);
            //trimg.FImageLink = fileBytes;
            //trimg.FImageLink = await ConvertFileToByteArrayAsync(file);
            trimg.FTeacherId = GetCurrentTeacherId();
            trimg.FImageSize = null;
            if (trimg.FImageName == null) { trimg.FImageName = "尚未命名"; }
            if (ModelState.IsValid)
            {
                //foreach (var file in Request.Form.Files)
                foreach (var file in f)
                {
                    if (file != null && file.Length > 0)
                    {
                        // 創建新的 TTeacherImage 對象
                        var newTrimg = new Models.TTeacherImage
                        {
                            FTeacherId = trimg.FTeacherId,
                            FImageSize = trimg.FImageSize,
                            FImageName = trimg.FImageName,
                            FCategory = trimg.FCategory,
                            // 在這裡添加其他屬性的設置
                        };

                        using (BinaryReader br = new BinaryReader(file.OpenReadStream()))
                        {
                            newTrimg.FImageLink = br.ReadBytes((int)file.Length);
                        }

                        //await ReadUploadImage(newTrimg);
                        _context.Add(newTrimg);
                        await _context.SaveChangesAsync();

                    }
                }
                return RedirectToAction("TRelatedPic");
                //return Redirect("~/TeacherAdmin/TRelatedPic");
            }
            return Content("新增失敗");
        }


        // GET: TeacherAdmin/TRelatedPic
        //動作簡述：回傳老師相關圖片
        [HttpGet]
        public IActionResult TRelatedPic()
        {
            int id = GetCurrentTeacherId();
            IEnumerable<TeacherBasicViewModel> vBasicVMCollection = new List<TeacherBasicViewModel>(
                _context.TTeacherImages
                  .Where(t => t.FTeacherId == id)
                .Select(t => new TeacherBasicViewModel
                {
                    TeacherImagesId = t.FTeacherImagesId,
                    TeacherId = id,
                    ImageName = t.FImageName,
                    ImageLink = t.FImageLink,
                    Category = t.FCategory,
                    TeacherImageModel = t,
                })
            );
            return View("TRelatedPic", vBasicVMCollection);
        }
        // GET: /TeacherAdmin/EditPartialViewInfo
        //動作簡述：顯示編輯畫面的資訊
        public IActionResult EditPartialViewInfo(int teacherImagesId)
        {
            var a = _context.TTeacherImages
                .Where(a => a.FTeacherImagesId == teacherImagesId)
                .Select(a => new { ImageName = a.FImageName, Category = a.FCategory, ImageId = a.FTeacherImagesId })
                .FirstOrDefault();
            return Json(a);
        }
        // GET: /TeacherAdmin/GetPicture/1
        //方法簡述：讀取資料庫的圖片
        public async Task<FileResult> GetPicture(int id)
        {
            //可能沒拿到值//抓的到有值 抓不到空值
            Models.TTeacherImage? c = await _context.TTeacherImages.FindAsync(id);
            byte[]? Content = c?.FImageLink;
            return File(Content, "image/jpeg");
        }

        // POST: TeacherAdmin/TdeletePic
        //動作簡述：刪除老師相關圖片(單張)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TdeletePic(int id)
        {
            var dimage = await _context.TTeacherImages.FindAsync(id);
            if (dimage != null)
            {
                _context.TTeacherImages.Remove(dimage);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(TRelatedPic));
        }

        // GET: TeacherAdmin/TeditPic
        //動作簡述：編輯單張圖片
        public async Task<IActionResult> TeditPic(Models.TTeacherImage trimg)
        {
            Models.TTeacherImage? dbTeacher = _context.TTeacherImages
                .Where(t => t.FTeacherImagesId == trimg.FTeacherImagesId).FirstOrDefault();
            if (dbTeacher == null)
            {
                return BadRequest("系統異常");
            }
            dbTeacher.FCategory = trimg.FCategory;
            dbTeacher.FImageName = trimg.FImageName;
            //trimg.FTeacherId = GetCurrentTeacherId();
            trimg = dbTeacher as Models.TTeacherImage;
            //驗證成功
            if (ModelState.IsValid)
            {
                //await ReadUploadImage(trimg);
                _context.Update(trimg);
                await _context.SaveChangesAsync();
                return RedirectToAction("TRelatedPic");
            }
            return Content("修改失敗");
        }
        //方法簡述：處理上傳的圖片
        private async Task ReadUploadImage(Models.TTeacherImage image)
        {
            //var files = Request.Form.Files["FImageLink"];
            //for(int i = 0; i < files.Length; i++)
            //{
            //    files[i]
            //}


            //因為try是捕捉傳到資料庫的錯誤，不是捕捉讀檔案的錯誤，所以寫到try裡外都可以
            if (Request.Form.Files["FImageLink"] != null)
            //如果有值
            {
                //建立物件放在using裡面，碰到又大括號後自動回收
                //OpenReadStream把檔案(Request.Form.Files["Picture"])開啟讀取串流
                using (BinaryReader br = new BinaryReader(Request.Form.Files["FImageLink"].OpenReadStream()))
                {
                    //把BinaryReader讀到的東西，放入Entity的參數:FImageLink
                    //ReadBytes 需要的參數是int //Length:檔案長度(型態是long)
                    image.FImageLink = br.ReadBytes((int)Request.Form.Files["FImageLink"].Length);
                }
            }
            else
            //如果沒值//如果user沒有換圖
            {
                return;
                //讀原來的FImageLink
                Models.TTeacherImage c = await _context.TTeacherImages.FindAsync(image.FTeacherImagesId);
                image.FImageLink = c.FImageLink;
                //c要解除追蹤//因為不能重複追蹤
                _context.Entry(c).State = EntityState.Detached;
            }
        }

        // /////////////////////未使用的動作及方法/////////////////////////////////
        private async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
        public IActionResult uploadimage(IFormFile file)
        {
            var fileDic = "Files";
            string filePath = Path.Combine(_hostEnv.WebRootPath, fileDic);
            if (!Directory.Exists(filePath)) { Directory.CreateDirectory(filePath); }
            var fileName = file.FileName;
            filePath = Path.Combine(filePath, fileName);
            using (FileStream fs = System.IO.File.Create(filePath))
            {
                file.CopyTo(fs);
            }
            var responseData = new
            {
                success = true,
                message = "檔案上傳成功",
                fileName = fileName,
                filePath = filePath
                // 其他您需要傳遞的資訊
            };

            // 回傳 JSON 資料
            return Json(responseData);
            //return RedirectToAction("TrelatedPic");
        }
        public byte[] ConvertIFormFileToByteArray(IFormFile file)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                file.CopyTo(ms);
                return ms.ToArray();
            }
        }
        // GET: TeacherAdmin/TEdit
        //動作簡述：產生編輯modal畫面
        public async Task<IActionResult> TEdit(int? id)
        {
            var teacherimage = await _context.TTeacherImages.FindAsync(id);
            return PartialView("T_EditTrPicPartial", teacherimage);
        }
        //■ ==========================     子謙作業區      ==========================■
        [HttpGet]
        public IActionResult VideoIntro()
        {
            return View();
        }
        [HttpGet]
        public IActionResult VEdit(int? id)
        {
            var videoList = _context.TVideoUploadUrls
            .Where(t => t.FVideoUploadUrlId == id)           
            .ToList();
            return View(videoList);
        }

        //API Get
        //獲得目前老師的影片資訊
        [HttpGet]
        public IActionResult GetVideoData()
        {
            //TVideoUploadUrl videoUploadUrl = _context.TVideoUploadUrls.FirstOrDefault(v => v.);
            var videoList = _context.TVideoUploadUrls
            .Where(t => t.FTeacher.FMemberId == GetCurrentMemberId())
            .Select(u => new {
                fVideoName = u.FVideoName,
                fVideoPath = u.FVideoPath,
                fUploadTime = u.FUploadTime,
                fVideoUploadUrlId = u.FVideoUploadUrlId
            })
            .ToList();
            return Json(new { data = videoList });
        }

        //API Delete
        //刪除目前影片
        [HttpDelete]
        public async Task<IActionResult> DeleteVideoData(int? id)
        {
            var video = await _context.TVideoUploadUrls.FirstOrDefaultAsync(v => v.FVideoUploadUrlId == id);
            if (video != null)
            {
                _context.TVideoUploadUrls.Remove(video);
                await _context.SaveChangesAsync(); 
            }
            return Json(new { success = true, message = "刪除成功" });
        }

        //■ ==========================     育蘋作業區      ==========================■
        public IActionResult OrderList()
        {
            return View();
        }
        public IActionResult ListDataJson2()
        {
            int currentTeacherId = GetCurrentTeacherId();
            if (currentTeacherId==0) 
            {
                return BadRequest("目前沒有老師登入");
            }
            var OrderData = from order in _context.TOrders
                            join orderDetail in _context.TOrderDetails on order.FOrderId equals orderDetail.FOrderId
                            join member in _context.TMembers on order.FMemberId equals member.FMemberId
                            join lessoncourse in _context.TLessonCourses on orderDetail.FLessonCourseId equals lessoncourse.FLessonCourseId
                            where lessoncourse.FTeacherId == currentTeacherId
                            select new OrderBasicViewModel
                            {
                                FOrderNumber = order.FOrderNumber,
                                FRealName = member.FRealName,
                                FPhone= member.FPhone,
                                FEmail = member.FEmail,
                                FOrderDate = order.FOrderDate,
                                FName = lessoncourse.FName,
                                FLessonPrice = lessoncourse.FPrice,
                                FOrderValid = orderDetail.FOrderValid,
                                FModificationDescription = orderDetail.FModificationDescription,
                            };
            return Json(new { data = OrderData });
        }  
    }
}
