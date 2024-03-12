using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.DTO;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Pkcs;
using System.Text;

namespace FinalGroupMVCPrj.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherAdminController : UserInfoController
    {
        private readonly LifeShareLearnContext _context;
        public TeacherAdminController(LifeShareLearnContext context)
        {
            _context = context;
        }
        //■ ==========================     翊妏作業區      ==========================■
        public string lessoncode = "";
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
                Time = (querystring.FEndTime.Value.TotalHours - querystring.FStartTime.Value.TotalHours).ToString() + "hr",
                MaxPeople = querystring.FMaxPeople,
                RegPeople = _context.TOrderDetails.Where(x => x.FLessonCourseId == querystring.FLessonCourseId).Count(),
                Status = querystring.FStatus,
                VenueType = querystring.FVenueType == true ? "實體" : "線上",
                lessonid = querystring.FLessonCourseId
            });
            return Json(new { data = lessons });
        }

        [HttpGet]

        public IActionResult LessonCreate()
        {

            return View("LCreate");
        }
        // POST: TeacherAdmin/LessonCreate
        [RequestFormLimits(MultipartBodyLengthLimit = 10240000)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LessonCreate( TLessonCourse lesson)
        {
            lesson.FTeacherId = GetCurrentTeacherId();
            //建立沒按功能鍵
            var count = _context.TLessonCourses.Where(x => x.FSubjectId == Convert.ToInt32(lesson.FSubject)).Count();
            lesson.FCode = lessoncode + (count+1);
            
            //lesson.FFiledid = Convert.ToInt32(lesson.FFiled);
            //FVenueType綁不動、全部asp-validation-for沒顯現

            
                await ReadUploadImage(lesson);
                _context.Add(lesson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(LessonList));
           
        }

        // GET: TeacherAdmin/LessonEdit
        [HttpGet]
        public IActionResult LessonEdit()
        {
            return View("LEdit");
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
                                            select new {cs.FSubjectId,cs.FSubjectName}
                                        ).Distinct().ToList();
            return Json(allowSubject);
        }
        [HttpGet]
        public IActionResult allCity()
        {
            var city = _context.TCities.Select(x=> new { x.FCityId, x.FCityName }).Distinct();
            return Json(city);
        }

        [HttpGet]
        public IActionResult allDistrict(int cityid)
        {
            var district = _context.TCityDistricts.Where(x=>x.FCityId== cityid).Select(x => new {x.FDistrictId,x.FDistrictName});
            return Json(district);
        }

        [HttpGet]
        public IActionResult loadCode(int filedid, int subjectid)
        {
            var code1 = _context.TCourseFields.Where(x => x.FFieldId == filedid).Select(x => x.FFieldCode).FirstOrDefault();
            var code2 = _context.TCourseSubjects.Where(x => x.FSubjectId == subjectid).Select(x => x.FSubjectCode).FirstOrDefault();
            string code = code1+code2;
            lessoncode = code;
            return Content(code, "text/plain", Encoding.UTF8);
        }
        public async Task<FileResult> showPicture(int id)
        {
            TLessonCourse? c = await _context.TLessonCourses.FindAsync(id);
            byte[]? content = c?.FPhoto;
            return File(content, "image/jpeg");
        }
        private async Task ReadUploadImage(TLessonCourse lesson)
        {
            if (Request.Form.Files["FPhoto"] != null)
            {
                //using自動資源管理
                using (BinaryReader br = new BinaryReader(
                    Request.Form.Files["FPhoto"].OpenReadStream()))
                {
                    lesson.FPhoto = br.ReadBytes((int)Request.Form.Files["FPhoto"].Length);
                }
            }
            else
            {
                //避免原本有圖的被覆蓋成預設沒圖
                TLessonCourse c = await _context.TLessonCourses.FindAsync(lesson.FLessonCourseId);
                lesson.FPhoto = c.FPhoto;
                //解除c的追蹤
                _context.Entry(c).State = EntityState.Detached;
            }
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
                        TeacherProfilePicURL = (t.FTeacherProfilePic != null) ? GetImageDataURL(t.FTeacherProfilePic) : "https://upload.wikimedia.org/wikipedia/commons/thumb/6/65/No-Image-Placeholder.svg/1665px-No-Image-Placeholder.svg.png",
                        Introduction = t.FIntroduction,
                        ContactInfo = t.FContactInfo,
                        Note = t.FNote,
                        SubjectName = t.TTeacherSubjects.Select(ts => ts.FSubject.FSubjectName),
                        TeacherModel = t
                    })
                );
            return View("TBasicinfo", vBasicVMCollection);
        }

        public IActionResult Edit(int TeacherID, [Bind("fTeacherId,fTeacherName,fIntroduction,fContactInfo,fNote")] TTeacher teacher)
        {
            if (TeacherID != teacher.FTeacherId) { return NotFound(); }
            if (ModelState.IsValid)
            {
                _context.Update(teacher);
                _context.SaveChanges();
                //重導到首頁
                return RedirectToAction("TBasicInfo");
            }
            return View(teacher);
        }



        // GET: TeacherAdmin/TRelatedPic
        //動作簡述：回傳老師相關圖片
        [HttpGet]
        public IActionResult TRelatedPic()
        {
            return View();
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
            .Select(u => new
            {
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


    }
}
