using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.DTO;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using OpenAI_API.Completions;
using OpenAI_API;
using System.Text;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using CloudinaryDotNet;
using OpenAI_API.Chat;
using Org.BouncyCastle.Asn1.Crmf;

namespace FinalGroupMVCPrj.Controllers
{
    [AllowAnonymous]
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

            ViewBag.teacherId = teacherId.ToString();
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
            ViewBag.memberId = studentId.ToString();
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
                .OrderByDescending(m => m.FPushCreatedTime)
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

        //GPT部分
        [HttpPost]
        public async Task<IActionResult> GetReplyByGPT([FromBody] string message)
        {
            string processedMessage = PreprocessInputMessage(message);

            string response = await GenerateResponse(processedMessage);

            return Ok(response);
        }

        private string PreprocessInputMessage(string message)
        {
            message = Regex.Replace(message, @"[^\p{IsCJKUnifiedIdeographs}\p{L}\s]", "");

            message = "用戶提到：" + message;

            // 如果消息包含投資理財相關的關鍵詞，則推薦大成老師的新手 ETF 選股課程
            if (message.Contains("投資理財") || message.Contains("理財課程"))
            {
                message = "請推薦以下課程給用戶，大成老師的課程：新手ETF選股課程，課程簡介：本課程旨在協助投資新手了解 ETF，選擇適合自己的 ETF，並學習有效的投資策略和選股技巧，以建立多元化的投資組合。 網址以<a href=\"https://localhost:7031/Lesson/Details/10041\">點此前往課程</a>呈現，請有條理的呈現，課程介紹不要太多，並將連結放在最後";
            }


            if (message.Contains("網站功能") || message.Contains("這個網站是做什麼的") || message.Contains("這個網站做甚麼"))
            {
                message += "\n\n回答：來學樂是一個多元化課程媒合的第三方整合平台，Life Share & Learn的發音與「來學樂」相似，學習使人快樂，我們期許為使用者創造便利又完善的學習環境。";
            }


            //if (message.Length > 500)
            //{
            //    message = message.Substring(0, 500);
            //}


            return message;
        }


        private async Task<string> GenerateResponse(string message)
        {
            var api = new OpenAIAPI("");

            var result = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
            {
                Model = OpenAI_API.Models.Model.GPT4,
                Temperature = 0.1,
                MaxTokens = 1000,
                Messages = new ChatMessage[] {
            new ChatMessage(ChatMessageRole.User, message)
                },
            });

            var reply = result.Choices[0].Message;

            return result.ToString();
        }
    }
}
