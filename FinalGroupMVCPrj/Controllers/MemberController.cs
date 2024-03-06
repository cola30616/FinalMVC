using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FinalGroupMVCPrj.Controllers
{
    public class MemberController : UserInfoController
    {
        private readonly LifeShareLearnContext _context;  //資料庫
        public MemberController(LifeShareLearnContext context)
        {
            _context = context;
        }
        // GET: Member/Setting
        //動作簡述：回傳設定會員資訊的頁面
        [HttpGet]
        public IActionResult Setting()
        {
            int memberId = GetCurrentMemberId();
            if (memberId == 0) { return Content("系統異常"); };
            var dbMember = _context.TMembers.Include(m=>m.TMemberCitiesLists).Include(m=>m.TMemberWishFields).FirstOrDefault(m => m.FMemberId == memberId);
            MemberDetailViewModel mdVm = new MemberDetailViewModel
            {
                Member = dbMember,
                Cities = dbMember?.TMemberCitiesLists.Select(c=>c.FCityId),
                WishFields = dbMember?.TMemberWishFields.Select(w => w.FFieldId),
            };
            ViewBag.JobSelectList = MemberDetailViewModel.GetJobSelectList();
            return View("OSetting", mdVm);
        }

        public IActionResult Setting2()
        {
            int memberId = GetCurrentMemberId();
            if (memberId == 0) { return Content("系統異常"); };
            var dbMember = _context.TMembers.Include(m => m.TMemberCitiesLists).Include(m => m.TMemberWishFields).FirstOrDefault(m => m.FMemberId == memberId);
            MemberDetailViewModel mdVm = new MemberDetailViewModel
            {
                Member = dbMember,
                Cities = dbMember?.TMemberCitiesLists.Select(c => c.FCityId),
                WishFields = dbMember?.TMemberWishFields.Select(w => w.FFieldId),
            };
            return Content(mdVm?.Cities?.Skip(1).First().ToString());
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

    }
}
