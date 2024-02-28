using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FinalGroupMVCPrj.Controllers
{

    public class UserInfoController : Controller
    {
        private readonly LifeShareLearnContext _context = new LifeShareLearnContext();
        //方法簡述：供繼承的控制器取得當前登入的會員ID
        protected int GetCurrentMemberId()
        {
            var idText = HttpContext.User.Claims.Where(u => u.Type == "MemberId").FirstOrDefault();
            if (idText != null)
            {
                return Convert.ToInt32(idText.Value);
            }
            return 0;
        }
        [AllowAnonymous]
        // GET: UserInfo/APICurrentMemberId
        //動作簡述：回傳當前登入的會員ID
        [HttpGet]
        public IActionResult APICurrentMemberId()
        {
            return Content(GetCurrentMemberId().ToString());
        }
        //方法簡述：供繼承的控制器取得指定會員資訊DTO
        protected MemberInfoDTO GetMemberInfo(int memberId)
        {
            var memberInfoDTO = new MemberInfoDTO();
            if (memberId != 0)
            {
                var dbMember = _context.TMembers.SingleOrDefault(m => m.FMemberId == memberId);

                if (dbMember != null)
                {
                    memberInfoDTO = new MemberInfoDTO
                    {
                        MemberId = dbMember.FMemberId,
                        RealName = dbMember.FRealName,
                        ShowName = dbMember.FShowName,
                        Email = dbMember.FEmail,
                        Phone = dbMember.FPhone ?? ""
                    };
                }
            }
            return memberInfoDTO;
        }
        //方法簡述：供繼承的控制器取得當前登入的會員資訊DTO
        protected MemberInfoDTO GetCurrentMemberInfo()
        {
           int currentMemberId = GetCurrentMemberId();
           return GetMemberInfo(currentMemberId);
        }

        // GET: UserInfo/APICurrentMemberId
        //動作簡述：回傳當前登入的會員資訊DTO json
        [HttpGet]
        public IActionResult APICurrentMemberInfo()
        {
            string json = JsonConvert.SerializeObject(GetCurrentMemberInfo());
            return Content(json, "application/json");
        }

        //方法簡述：供繼承的控制器取得當前登入會員的老師ID(不是老師或停權為0)
        protected int GetCurrentTeacherId()
        {
            int currentMemberId = GetCurrentMemberId();
            var dbMember = _context.TMembers.SingleOrDefault(m => m.FMemberId == currentMemberId);
            if (dbMember != null && dbMember.FQualifiedTeacher)
            {
                int teacherId = 0;
                var dbTeacher = _context.TTeachers.SingleOrDefault(t => t.FMemberId == currentMemberId);
               if(dbTeacher != null)
                {
                    teacherId = dbTeacher.FTeacherId;
                    return teacherId;
                }
            }
            return 0;
        }
        // GET: UserInfo/APICurrentMemberId
        //動作簡述：回傳當前登入會員的老師ID(不是老師或停權為0)
        public IActionResult APICurrentTeacherId()
        {
            return Content(GetCurrentTeacherId().ToString());
        }
        //方法簡述：供繼承的控制器取得指定老師資訊DTO
        protected TeacherInfoDTO GetTeacherInfo(int teacherId)
        {
            var teacherIdInfoDTO = new TeacherInfoDTO();
            if (teacherId != 0)
            {
                var dbTeacher = _context.TTeachers.SingleOrDefault(t => t.FTeacherId == teacherId);

                if (dbTeacher != null)
                {
                    teacherIdInfoDTO = new TeacherInfoDTO
                    {
                        MemberId = dbTeacher.FMemberId,
                      TeacherId = dbTeacher.FTeacherId,
                      TeacherName = dbTeacher.FTeacherName
                    };
                }
            }
            return teacherIdInfoDTO;
        }
        // GET: UserInfo/APITeacherInfo
        //動作簡述：回傳指定老師資訊DTO json
        [HttpGet]
        public IActionResult APITeacherInfo(int teacherId)
        {
            string json = JsonConvert.SerializeObject(GetTeacherInfo(teacherId));
            return Content(json, "application/json");
        }

    }
}
