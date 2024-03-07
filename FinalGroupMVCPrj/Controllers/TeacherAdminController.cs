﻿using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.DTO;
using FinalGroupMVCPrj.Models.ViewModel;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Cryptography.Pkcs;
using static NuGet.Packaging.PackagingConstants;

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
        // GET: TeacherAdmin/LessonList
        //動作簡述：回傳老師課程清單資訊
       
        
        [HttpGet]
        public IActionResult LessonList( )
        {
            return View();
        }

        // GET: TeacherAdmin/LessonCreate
        [HttpGet]
        public IActionResult LessonCreate()
        {
            return View("LCreate");
        }

        // GET: TeacherAdmin/LessonEdit
        [HttpGet]
        public IActionResult LessonEdit()
        {
            return View("LEdit");
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
                    .Select(t =>new TeacherBasicViewModel {
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
        public IActionResult ListDataJson()
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
                            select new 
                            {
                                OrderID = order.FOrderId,
                                RealName = member.FRealName,
                                Email = member.FEmail,
                                OrderDate = order.FOrderDate,
                                Name = lessoncourse.FName,
                                Price = lessoncourse.FPrice,
                                OrderValid = orderDetail.FOrderValid,
                                ModificationDescription = orderDetail.FModificationDescription,
                            };
            return Json(new { data = OrderData });
        }
       
    }
}
