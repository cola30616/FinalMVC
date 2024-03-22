using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.DTO;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;

namespace FinalGroupMVCPrj.Controllers
{
    public class MessageController : UserInfoController
    {
        private readonly LifeShareLearnContext _context;
        public MessageController(LifeShareLearnContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> ChatTeacher()
        {
            int teacherId = GetCurrentTeacherId();
            ViewBag.FTeacherId = teacherId;
            var query = _context.TLessonCourses
                                                 .Include(course => course.TOrderDetails)
                                                 .ThenInclude(od => od.FOrder)
                                                 .ThenInclude(od => od.FMember)
                                                 .Where(c => c.FTeacherId == teacherId)
                                                 .Where(od => od.TOrderDetails.Any(od => od.FOrderValid == true))
                                                 .Select(od => od.TOrderDetails.Select(od => od.FOrder).Select(m => m.FMember)
                                                 .Select(m => new ChatTeacherViewModel
                                                 {
                                                     FMemberId = m.FMemberId,
                                                     FShowName = m.FShowName,
                                                     FMemberProfilePic = m.FMemberProfilePic,
                                                 })).SelectMany(m => m).Distinct().ToList();
            var query2 = query;

            ViewBag.teacherId = teacherId;
            return View("ChatTeacher", query);
        }

        public IActionResult ChatStudent()
        {
            int studentId = GetCurrentMemberId();
            var query = _context.TOrderDetails
                .Include(od => od.FOrder)
                .ThenInclude(o => o.FMember)
                .Include(od => od.FLessonCourse)
                .ThenInclude(lc => lc.FTeacher)
                .Where(od => od.FOrder.FMemberId == studentId)
                .Select(msg => new ChatTeacherViewModel
                {
                    FTeacherId = msg.FLessonCourse.FTeacherId,
                    FTeacherName = msg.FLessonCourse.FTeacher.FTeacherName,
                    FMemberProfilePic = msg.FLessonCourse.FTeacher.FTeacherProfilePic
                })
                .Distinct();
            ViewBag.memberId = studentId;
            return View("ChatStudent", query);
        }

        [HttpGet]
        //獲取照片
        public async Task<FileResult> GetTeacherPicture(int FTeacherId)
        {
            Models.TTeacher? teacher = await _context.TTeachers.FindAsync(FTeacherId);
            if (teacher.FTeacherProfilePic != null)
            {
                byte[]? Content = teacher?.FTeacherProfilePic;
                return File(Content, "image/jpeg");
            }
            return File("images/OwenAdd/memberNoPhoto.jpg", "image/jpeg");
        }

        [HttpGet]
        public async Task<FileResult> GetStudentPicture(int fMemberId)
        {
            Models.TMember? member = await _context.TMembers.FindAsync(fMemberId);
            if (member.FMemberProfilePic != null)
            {
                byte[]? Content = member?.FMemberProfilePic;
                return File(Content, "image/jpeg");
            }
            return File("images/OwenAdd/memberNoPhoto.jpg", "image/jpeg");
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentInfo(int fMemberId)
        {
            Models.TMember? member = await _context.TMembers.FindAsync(fMemberId);

            return Json(member);
        }

        [HttpGet]
        public new async Task<IActionResult> GetTeacherInfo(int fTeacherId)
        {
            Models.TTeacher? teacher = await _context.TTeachers.FindAsync(fTeacherId);

            return Json(teacher);
        }

        public IActionResult PushMsgList()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ListDataJson()
        {
            var pushMsgList = _context.TPushMessages.AsQueryable();

            IEnumerable<PushMessageViewModel> pushmsgVMCollection =
                  new List<PushMessageViewModel>(
                            pushMsgList.Select(m => new PushMessageViewModel
                            {
                                FPushMessageId = m.FPushMessageId,
                                FPushType = m.FPushType,
                                FPushStartDate = m.FPushStartDate,
                                FPushEndDate = m.FPushEndDate,
                                FPushContent = m.FPushContent,
                                FPushImagePath = m.FPushImagePath,
                                FPushCreatedTime = m.FPushCreatedTime,
                                FPushLastUpdatedTime = m.FPushLastUpdatedTime,
                            })); ;
            return Json(new { data = pushmsgVMCollection });
        }

        public IActionResult Valuate()
        {
            return PartialView("_BValuatePartial");
        }

        [HttpPost]
        public async Task<IActionResult> CreatePushMsg(CreatePushMsgViewModel pmsg)
        {
            TPushMessage msg = new TPushMessage();
            try
            {
                if (pmsg.FPushImagePath != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await pmsg.FPushImagePath.CopyToAsync(memoryStream);
                        msg.FPushImagePath = memoryStream.ToArray();
                    }
                }
                msg.FEmployeeId = 1;
                msg.FPushContent = pmsg.FPushContent;
                msg.FPushType = pmsg.FPushType;
                msg.FPushCreatedTime = DateTime.Now;
                msg.FPushStartDate = DateTime.Now.Date;
                msg.FPushEndDate = DateTime.Now.Date;
                msg.FPushLastUpdatedTime = DateTime.Now;
                msg.FPushLayoutId = 1;
                msg.FPushMethod = "notification";
                if (ModelState.IsValid)
                {
                    _context.TPushMessages.Add(msg);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(PushMsgList));
                }
                return StatusCode(500, "系統異常");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "系統異常" + ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPushMsgList(int PutmsgId)
        {
            var query = await _context.TMembers.ToListAsync();
            ViewBag.PutmsgId = PutmsgId;
            return PartialView("_BPushMsgToMemberPartial", query);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePush([FromBody] PushMsgDTO pushMsg)
        {
            foreach (var member in pushMsg.selectedMembers)
            {
                TMemberGetPush push = new TMemberGetPush();
                push.FPushMessageId = pushMsg.pushMsgId;
                push.FMemberId = member;
                push.FPushCreatedTime = DateTime.Now.AddSeconds(pushMsg.pushDelay);
                push.FPushRead = false;
                _context.TMemberGetPushes.Add(push);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetPushJson(int memberId)
        {
            var query = await _context.TMemberGetPushes
                .Include(m => m.FPushMessage)
                .Where(m => m.FMemberId == memberId)
                .OrderBy(m => m.FPushCreatedTime)
                .Select(m => new PushMessageViewModel
                {
                    FPushMessageId = m.FPushMessageId,
                    FPushContent = m.FPushMessage.FPushContent,
                    FPushImagePath = m.FPushMessage.FPushImagePath,
                    FPushCreatedTime = m.FPushCreatedTime
                })
                .ToListAsync();
            return Json(new { data = query });
        }
    }
}
