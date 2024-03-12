using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.ViewModel;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalGroupMVCPrj.Controllers
{
    public class OrderController : UserInfoController
    {
        private readonly LifeShareLearnContext _context;
        public OrderController(LifeShareLearnContext context)
        {
            _context = context;
        }

        //■ ==========================     Apple 作業區      ==========================■

        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // 取得TLessonCourse資料
            var lessonCourse = _context.TLessonCourses.FirstOrDefault(lc => lc.FLessonCourseId == id);
            if (lessonCourse == null)
            {
                return NotFound();
            }

            // 取得TMember資料
            TMember? member = _context.TMembers.FirstOrDefault(m => m.FMemberId == GetCurrentMemberId()); 
            if (member == null)
            {
                return NotFound();
            }

            // 將得到的資料放到OrderDetailViewModel
            var kViewModel = new OrderDetailViewModel
            {
                lessonCourse = lessonCourse,
                member = member
            };
            return View("OrderDetailViewModel",member);
        }                  
    }
}


//[HttpGet]
//public async Task<IActionResult> Details(int? id)
//{

//    var detail = await _context.TLessonCourses
//    .Include(order => order.TOrderDetails)
//        .ThenInclude(evaluation => evaluation.TLessonEvaluations)
//    .Where(eval => eval.FLessonCourseId == id)
//    .Select(querystring => new LessonDetailViewModel
//    {
//        //主頁部分
//        FTeacher = querystring.FTeacher,
//        FName = querystring.FName,
//        FFiled = _context.TCourseFields.Where(x => x.FFieldId == querystring.FSubject.FFieldId).Select(x => x.FFieldName).FirstOrDefault(),
//        FSubject = querystring.FSubject,
//        FVenueType = querystring.FVenueType,
//        FRegPeople = _context.TOrderDetails.Where(x => x.FLessonCourseId == id).Count(),
//        FPrice = querystring.FPrice,
//        FTime = ((querystring.FEndTime.Value.TotalHours - querystring.FStartTime.Value.TotalHours)).ToString(),
//        FMaxPeople = querystring.FMaxPeople,
//        FMinPeople = querystring.FMinPeople,
//        FVenueName = querystring.FVenueName,
//        FCity = _context.TCities.Where(c => c.TCityDistricts.Any(d => d.FDistrictId == querystring.FDistrictId)).Select(x => x.FCityName).FirstOrDefault(),
//        FDistrict = _context.TCityDistricts.Where(x => x.FDistrictId == querystring.FDistrictId).Select(x => x.FDistrictName).FirstOrDefault(),
//        FAddressDetail = querystring.FAddressDetail,
//        FOnlineLink = querystring.FOnlineLink,
//        FLessonDate = querystring.FLessonDate,
//        FStartTime = querystring.FStartTime,
//        FEndTime = querystring.FEndTime,
//        FRegDeadline = querystring.FRegDeadline,
//        FDescription = querystring.FDescription,
//        FEditorDes = querystring.FEditorDes,
//        FRequirement = querystring.FRequirement,

//        //// 評價部分
//        //FAvgScore = querystring.TOrderDetails.SelectMany(order => order.TLessonEvaluations).Any()? 
//        //Math.Round(querystring.TOrderDetails.SelectMany(order => order.TLessonEvaluations).Average(evaluation => evaluation.FScore), 1) :0
//    }).ToListAsync();

//    return View("LDetails", detail[0]);
//}
//
//
//
//
//var detail = _context.TLessonCourses
            //.Where(eval => eval.FLessonCourseId == id);
            //OrderDetailViewModel OrderDetailViewModel = new OrderDetailViewModel
            //{
            //  FName = detail.Select(x=>x.FName).ToString(),
            //  FDescription = detail.Select(x=>x.FDescription).ToString()
            //};



