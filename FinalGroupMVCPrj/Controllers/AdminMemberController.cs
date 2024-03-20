using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.DTO;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using CsvHelper;
using System.ComponentModel.DataAnnotations;
using Azure;

namespace FinalGroupMVCPrj.Controllers
{
    [AllowAnonymous]
    public class AdminMemberController : Controller
    {
        private readonly LifeShareLearnContext _context;
        public AdminMemberController(LifeShareLearnContext context) 
        {
            _context = context;
        }
        public IActionResult List()
        {
            return View();
        }
        public IActionResult ListDataJson([FromBody] MemberFilterDTO? memberFilterData)
        {
            var memberList = _context.TMembers.AsQueryable();
            if (memberFilterData != null)
            {

            }
            IEnumerable<MemberBasicViewModel> mBasicVMCollection =
                  new List<MemberBasicViewModel>(
                            memberList.Select(m => new MemberBasicViewModel
                            {
                                MemberId = m.FMemberId,
                                Email = m.FEmail,
                                EmailVerification = m.FEmailVerification ? "已驗證" : "未驗證",
                                RealName = m.FRealName,
                                ShowName = m.FShowName,
                                GetCampInfo = m.FGetCampaignInfo ? "是" : "否",
                                RegisterDateTime = m.FRegisterDatetime.ToString("yyyy/MM/dd HH:mm"),
                                Status = m.FStatus == true ? "正常" : "停權中"
                            })); ;
            return Json(new{data = mBasicVMCollection});
        }
        public IActionResult LoginHistory(int memberId, int toSkip, DateTime? lastDate)
        {
            var q = _context.TMemberLoginLogs.Where(l => l.FMemberId == memberId);
            if(lastDate != null)
            {
                q = q.Where(l => l.FLoginDateTime <= lastDate);
            }

            var   loginHistory =  q.OrderByDescending(m=>m.FLoginDateTime).Skip(toSkip).Take(5).ToList();

            return Json(loginHistory);
        }
        // GET: AdminMember/Detail/1
        //動作簡述：回傳會員詳細資訊 AdminMemberDTO
        [HttpGet]
        public IActionResult Detail(int id)
        {
            int memberId = id;
            if (memberId == 0) { return BadRequest("沒有指定會員"); };
            var dbMember = _context.TMembers.Include(m => m.TMemberCitiesLists).Include(m => m.TMemberWishFields).FirstOrDefault(m => m.FMemberId == memberId);
            if (dbMember == null) { return StatusCode(500,"資料庫系統異常"); };
            try
            {
                AdminMemberDTO mdVm = new AdminMemberDTO
                {
                    MemberId = dbMember.FMemberId,
                    RegDate = dbMember.FRegisterDatetime.ToString("yyyy/MM/dd HH:mm"),
                    RealName = dbMember.FRealName,
                    ShowName = dbMember.FShowName,
                    Email = dbMember.FEmail,
                    EmailVerification = dbMember.FEmailVerification?"已驗證":"未驗證",
                    Phone = dbMember.FPhone,
                    GetCampInfo = dbMember.FGetCampaignInfo?"是":"否",
                    Birth = dbMember.FBirthDate?.ToString("yyyy-MM-dd"),
                    Gender = dbMember.FGender == true ? "男" : (dbMember.FGender == false ? "女" : "其他／不便透露"),
                    Job = dbMember.FJob,
                    Education = dbMember.FEducation,
                    Note = string.IsNullOrEmpty(dbMember.FNote)?"未連結任何帳號":"LINE" ,
                    Status = dbMember.FStatus?"正常":"停權中",
                    Cities =
                    dbMember.TMemberCitiesLists.Join(
                        _context.TCities, m => m.FCityId, c => c.FCityId,
                        (m, c) => c.FCityName),
                    WishFields =
                                        dbMember.TMemberWishFields.Join(
                        _context.TCourseFields, m => m.FFieldId, f => f.FFieldId,
                        (m, f) => f.FFieldName)
                };
            return Ok(mdVm );
            }catch (Exception ex)
            {
                return StatusCode(500, "系統異常："+ex);
            }

        }

        // GET: AdminMember/MemberPhoto
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
        // POST: AdminMember/RemoveMemberPhoto
        //動作簡述：回傳會員頭像的url
        [HttpPost]
        public async Task<IActionResult> RemoveMemberPhoto(int id)
        {
            try
            {
                if(id == 0) { return BadRequest("沒有指定會員"); }
                TMember? dbMember = await _context.TMembers.FirstOrDefaultAsync(m => m.FMemberId == id);
                if(dbMember == null) { return StatusCode(500, "資料庫系統異常請稍後再試" ); }
                dbMember.FMemberProfilePic = null;
                _context.TMembers.Update(dbMember);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500,"系統異常請稍後再試："+ex);
            }
        }


        public async Task<IActionResult> DownloadCsv()
        {
            try
            {
               //要輸出CSV的資料
                var data = await _context.TMembers.Include(m => m.TMemberCitiesLists).Include(m => m.TMemberWishFields).Select(m =>
                 new AdminMemberCSV
                 {
                     MemberId = m.FMemberId,
                     RegDate = m.FRegisterDatetime.ToString("yyyy/MM/dd HH:mm"),
                     RealName = m.FRealName,
                     ShowName = m.FShowName,
                     Email = m.FEmail,
                     EmailVerification = m.FEmailVerification ? "已驗證" : "未驗證",
                     Phone = m.FPhone,
                     Status = m.FStatus ? "正常" : "停權中",
                     GetCampInfo = m.FGetCampaignInfo ? "是" : "否",
                     Birth = m.FBirthDate == null ? "" : ((DateTime)m.FBirthDate).ToString("yyyy-MM-dd"),
                     Gender = m.FGender == true ? "男" : (m.FGender == false ? "女" : "其他／不便透露"),
                     Job = m.FJob,
                     Education = m.FEducation,
                     Note = string.IsNullOrEmpty(m.FNote) ? "未連結任何帳號" : "LINE",
                     Cities = String.Join("／",
                    m.TMemberCitiesLists.Join(
                        _context.TCities, m => m.FCityId, c => c.FCityId,
                        (m, c) => c.FCityName)),
                     WishFields =
                                        String.Join("／", m.TMemberWishFields.Join(
                        _context.TCourseFields, m => m.FFieldId, f => f.FFieldId,
                        (m, f) => f.FFieldName))
                 }).ToListAsync();

                // CSV MemoryStream
                using (var memoryStream = new MemoryStream())
                using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
                using (var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture))
                {

                    // 寫入資料
                    csv.WriteRecords(data);
                    writer.Flush();
                    memoryStream.Position = 0;

                    // 回傳CSV文件
                    return File(memoryStream.ToArray(), "text/csv", $"{DateTime.Now.ToString("yyyy-MM-dd-HH-mm")}"+ "_會員資料.csv");
                }
            }
            catch (Exception ex)
            {
                // 在控制台输出异常信息
                Console.WriteLine($"Error generating CSV file: {ex.Message}");
                // 返回适当的错误信息给用户
                return StatusCode(StatusCodes.Status500InternalServerError, "CSV下載失敗，請稍後再試");
            }
        }
        // POST: AdminMember/ChangeValue/1
        //動作簡述：更改會員值
        [HttpPost]
        public async Task<IActionResult> ChangeValue(int id, string property, string changeTo)
        {
            try
            {
                if (id == 0 || string.IsNullOrEmpty(property) || string.IsNullOrEmpty(changeTo))
                {
                    return BadRequest("沒有參數或系統異常");
                }
                TMember? dbMember = await _context.TMembers.FirstOrDefaultAsync(m => m.FMemberId == id);
                if (dbMember == null) { return BadRequest("會員不存在或系統異常"); }
                if (property == "status")
                {
                    if (changeTo == "停權") { dbMember.FStatus = false; }
                    else if (changeTo == "恢復") { dbMember.FStatus = true; }
                }else  if(property == "realName")
                {
                    dbMember.FRealName = changeTo;
                }
                else if (property == "showName")
                {
                    dbMember.FShowName = changeTo;
                }
                else if (property == "phone")
                {
                    dbMember.FPhone = changeTo;
                }
                else if (property == "birth")
                {
                    try
                    {
                        dbMember.FBirthDate = DateTime.Parse(changeTo);
                    }catch (Exception ex)
                    {
                        return StatusCode(500, "系統異常：" + ex);
                    }
                 
                }
                _context.TMembers.Update(dbMember);
                await _context.SaveChangesAsync();
                return Ok();
            }catch (Exception ex)
            {
                return StatusCode(500, "系統異常：" + ex);
            }
        }
    }
}
