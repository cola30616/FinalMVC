using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.DTO;
using FinalGroupMVCPrj.Models.ViewModels;
using FinalGroupMVCPrj.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FinalGroupMVCPrj.Controllers
{
    public class MemberController : UserInfoController
    {
        private readonly LifeShareLearnContext _context;  //資料庫
        private readonly IMailService _mailService;
        public MemberController(LifeShareLearnContext context, IMailService _MailService)
        {
            _context = context;
            _mailService = _MailService;
        }
        // GET: Member/Setting
        //動作簡述：回傳設定會員資訊的頁面
        [HttpGet]
        public IActionResult Setting()
        {
            int memberId = GetCurrentMemberId();
            if (memberId == 0) { return Content("無會員登入中"); };
            var dbMember = _context.TMembers.Include(m=>m.TMemberCitiesLists).Include(m=>m.TMemberWishFields).FirstOrDefault(m => m.FMemberId == memberId);
            if (dbMember == null) { return Content("登入系統異常"); };
            MemberDetailViewModel mdVm = new MemberDetailViewModel
            {
                Member = dbMember,
                Cities = dbMember?.TMemberCitiesLists.Select(c=>c.FCityId),
                WishFields = dbMember?.TMemberWishFields.Select(w => w.FFieldId),
            };
            //會員單選/多選資料比對後給view應該被select的
            //性別(單選)
            IEnumerable<SelectListItem> GenderSelectList = new List<SelectListItem>
                {
                new SelectListItem { Text = "其他", Value = null },
                new SelectListItem { Text = "女", Value = "false" },
     new SelectListItem  { Text = "男", Value = "true"}
                 };
            foreach (var s in GenderSelectList)
            {
                if (s.Value == dbMember.FGender.ToString())
                {
                    s.Selected = true;
                    break;
                }
            }
            ViewBag.GenderSelectList = GenderSelectList;
            //職業類別(單選)
            var JobSelectList = MemberDetailViewModel.GetJobSelectList();
            foreach(var s in JobSelectList)
            {
                if(s.Value == dbMember.FJob)
                {
                    s.Selected = true;
                    break;
                }
            }
            ViewBag.JobSelectList = JobSelectList;
            //最高學歷(單選)
            var EduSelectList = MemberDetailViewModel.GetEduSelectList();
            foreach (var s in EduSelectList)
            {
                if (s.Value == dbMember.FEducation)
                {
                    s.Selected = true;
                    break;
                }
            }
            ViewBag.EduSelectList = EduSelectList;
            //生活縣市(多選)
            var CitySelectList = _context.TCities.Select(c => new SelectListItem { Text = c.FCityName, Value = c.FCityId.ToString() }).ToList();
            if(mdVm.Cities != null)
            {
            foreach (var c in CitySelectList)
            {
                if (mdVm.Cities.Any(mc=>mc.ToString()==c.Value))
                {
                    c.Selected = true;
                }
            }
            }
            ViewBag.CitySelectList = CitySelectList;
            //想學的領域(多選)
            var FieldSelectList = _context.TCourseFields.Select(f => new SelectListItem { Text = f.FFieldName, Value = f.FFieldId.ToString() }).ToList();
            if (mdVm.WishFields != null)
            {
                foreach (var f in FieldSelectList)
                {
                    if (mdVm.WishFields.Any(mf => mf.ToString() == f.Value))
                    {
                        f.Selected = true;
                    }
                }
            }
            ViewBag.FieldSelectList = FieldSelectList;
            return View("OSetting", mdVm);
        }
        // POST: Member/SettingSave
        //動作簡述：儲存會員修改的資訊
        [HttpPost]
        public IActionResult SettingSave([FromBody]MemberDetailViewModel mdVm)
        {
            int memberId = GetCurrentMemberId();
            var dataMember = mdVm?.Member;
            if (mdVm ==null || dataMember == null||mdVm.Cities==null||mdVm.WishFields==null) { return BadRequest("提交會員資料異常"); }
            if (dataMember.FMemberId != memberId) { return BadRequest("不合法的會員提交"); }
            var dbmember = _context.TMembers.SingleOrDefault(m=> m.FMemberId== memberId);
            if(dbmember == null) { return StatusCode(500, "資料庫系統異常"); }
            if(dbmember.FBirthDate == null && dataMember.FBirthDate != null) { dbmember.FBirthDate = dataMember.FBirthDate; }
            dbmember.FShowName = dataMember.FShowName;
            dbmember.FPhone = dataMember.FPhone;
            dbmember.FGender = dataMember.FGender;
            dbmember.FEducation = dataMember.FEducation;
            dbmember.FJob = dataMember.FJob;
            var dbMemCitiesId = _context.TMemberCitiesLists.Where(m => m.FMemberId == memberId).Select(m=>m.FCityId).ToList() ;
            var toDeteteCitiesId = dbMemCitiesId.Except(mdVm.Cities);
            var toAddCitiesId = mdVm.Cities.Except(dbMemCitiesId);
            foreach(int d in toDeteteCitiesId) 
            {
                var toDelete = _context.TMemberCitiesLists.FirstOrDefault(m => m.FCityId == d && m.FMemberId == memberId);
                    _context.TMemberCitiesLists.Remove(toDelete);
             }
            foreach (int a in toAddCitiesId)
            {
                var toAdd = new TMemberCitiesList { FMemberId = memberId, FCityId = a };
                _context.TMemberCitiesLists.Add(toAdd);
            }
            var dbMemFieldsId = _context.TMemberWishFields.Where(m => m.FMemberId == memberId).Select(m => m.FFieldId).ToList();
            var toDeteteFieldsId = dbMemFieldsId.Except(mdVm.WishFields);
            var toAddFieldsId = mdVm.WishFields.Except(dbMemFieldsId);
            foreach (int d in toDeteteFieldsId)
            {
                var toDelete = _context.TMemberWishFields.FirstOrDefault(m => m.FFieldId == d && m.FMemberId == memberId);
                _context.TMemberWishFields.Remove(toDelete);
            }
            foreach (int a in toAddFieldsId)
            {
                var toAdd = new TMemberWishField { FMemberId = memberId, FFieldId = a };
                _context.TMemberWishFields.Add(toAdd);
            }
            try
            {
                _context.TMembers.Update(dbmember);
                _context.SaveChanges();
                return Ok();
            }catch(Exception e)
            {
                return StatusCode(500, "資料庫系統異常："+e);
            }
            
           
        }

        // GET: Member/Fav
        //動作簡述：回傳設定會員資訊的頁面
        [HttpGet]
        public IActionResult Fav()
        {
            return View("Fav");
        }

        // GET: Member/MemberPhoto
        //動作簡述：回傳會員頭像的url
        [HttpGet]
        public async Task<IActionResult> MemberPhoto(int? id)
        {
            string blobDataURL = "";
            TMember? member = await _context.TMembers.FirstOrDefaultAsync(m => m.FMemberId == id);
            byte[]? image = member?.FMemberProfilePic;
            if (image == null || image.Length == 0)
            {
                blobDataURL = "";

            }
            else
            {
                string base64String = Convert.ToBase64String(image);
                blobDataURL = $"data:image/jpeg;base64,{base64String}";
            }
            return Content(blobDataURL);
        }
        // POST: Member/UpdatePhoto
        //動作簡述：更新會員的頭像
        [HttpPost]
        public async Task<IActionResult> UpdatePhoto(IFormFile file)
        {
            if(file==null || file.Length == 0)
            {
                return BadRequest("沒有檔案上傳");
            }
            int memberId = GetCurrentMemberId();
            if(memberId == 0) { return BadRequest("沒有會員登入中"); }
            var dbMember = _context.TMembers.SingleOrDefault(m => m.FMemberId == memberId);
                if (dbMember == null) { return StatusCode(500, "資料庫系統異常"); }
            byte[] fileBytes = null;
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                fileBytes = memoryStream.ToArray();
            }
            if(fileBytes == null) {
                return StatusCode(500, "記憶體轉換異常");}
            dbMember.FMemberProfilePic = fileBytes;
            _context.TMembers.Update(dbMember);
            try
            {
                await _context.SaveChangesAsync();
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, "系統異常：" + ex);
            }
        }
        // POST: Member/SaveLoginLog
        //動作簡述：儲存登入資訊
        [HttpPost]
        public async Task<IActionResult> SaveLoginLog(string ip, string geoInfo, string browerNOs)
        {
            try
            {
                int memberId = await Task.Run(() => GetCurrentMemberId());
                if (memberId == 0) { return StatusCode(500, "登入系統異常"); }
                TMemberLoginLog memberLoginLog = new TMemberLoginLog
                {
                    FMemberId = memberId,
                    FLoginDateTime = DateTime.Now,
                    FLoginIp = ip,
                    FLoginGeoInfo = geoInfo,
                    FLoginBrowerNos = browerNOs,
                };
                _context.TMemberLoginLogs.Add(memberLoginLog);
                await _context.SaveChangesAsync();
                return Ok();
            }catch (Exception ex)
            {
                    return StatusCode(500, $"系統異常：{ex}");
             }
        }

        [AllowAnonymous]
        // POST: Member/SignUp
        //動作簡述：儲存登入資訊
        [HttpPost]
        public async Task<IActionResult> SignUp(TMember member)
        {
            try
            {
                var dbMember = _context.TMembers.SingleOrDefault(m=> m.FEmail == member.FEmail);
                if(dbMember != null) { return BadRequest("此信箱已有人使用"); }
                string beforePassword = member.FPassword;
                string afterPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(beforePassword, 13);
                TMember newMember = new TMember
                {
                    FRealName = member.FRealName,
                    FShowName = member.FRealName,
                    FPassword = afterPassword,
                    FRegisterDatetime = DateTime.Now,
                    FEmail = member.FEmail,
                    FEmailVerification = false,
                    FGetCampaignInfo = member.FGetCampaignInfo,
                    FQualifiedTeacher = false,
                    FStatus = true
                };
                _context.TMembers.Add(newMember);
                await _context.SaveChangesAsync();
                return Ok("註冊成功請驗證信箱");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"系統異常：{ex}");
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public bool SendEmail(MailData mailData)
        {
            return _mailService.SendMail(mailData);
        }
    }
}
