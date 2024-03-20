using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FinalGroupMVCPrj.Controllers
{
    public class LessonHistory : UserInfoController
    {
        private readonly LifeShareLearnContext _context;
        public LessonHistory(LifeShareLearnContext context)
        {
            _context = context;
        }
        //■ ==========================     子謙作業區      ==========================■
        // GET: LessonHistory/List
        //動作簡述：回傳課程記錄清單的頁面
        [HttpGet]
        public IActionResult LearningRecord()
        {            
            var successRecord = _context.TOrderDetails.Where(lr => lr.FOrder.FMemberId == GetCurrentMemberId() && lr.FOrderValid == true)
                .Select(lr => lr.FLessonCourse).Distinct().ToList();
            var cancelRecord = _context.TOrderDetails.Where(lr => lr.FOrder.FMemberId == GetCurrentMemberId() && lr.FOrderValid == false)
                .Select(lr => lr.FLessonCourse).Distinct().ToList();
            //var recordDictionary = successRecord
            //.GroupBy(lr => lr.TOrderDetails)
            //.ToDictionary(
            //    group => group.Key, // 使用 orderID 作為字典的鍵
            //    group => group.Select(lr => lr.FLessonCourse).FirstOrDefault() // 將每個組的 TLessonCourse 映射為字典值
            //);
            LearningRecordVM learningRecord = new()
            {
                SuccessRecord = successRecord,
                CancelRecord = cancelRecord,

            };
            return View(learningRecord);
        }



        //■ ==========================     Apple 作業區      ==========================■
        public IActionResult Details()
        {
            ViewBag.FOrderDetailId = 5;
            return View();
        }
    }
}
