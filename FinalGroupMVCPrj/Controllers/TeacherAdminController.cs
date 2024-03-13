using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.DTO;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Security.Cryptography.Pkcs;

namespace FinalGroupMVCPrj.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherAdminController : UserInfoController
    {
        private IWebHostEnvironment _hostEnv;
        private readonly LifeShareLearnContext _context;
        public TeacherAdminController(LifeShareLearnContext context, IWebHostEnvironment env)
        {
            _context = context;
            _hostEnv = env;
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
        // POST: TeacherAdmin/TEdittrbasicprofile
        //動作簡述：編輯老師基本資料(不包含老師頭像)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TEdittrbasicprofile(TTeacher teacher)
        {
            // https://www.google.com/search?q=.net+core+mva+update+partial+model+property&rlz=1C1ONGR_zh-TWTW1027TW1027&oq=.net+core+mva+update+partial+model+property&gs_lcrp=EgZjaHJvbWUyBggAEEUYOdIBCTMwMzYzajBqMagCALACAA&sourceid=chrome&ie=UTF-8#ip=1
            // ======== 局部更新資料 start =========
            TTeacher? dbTeacher = _context.TTeachers.Where(t => t.FTeacherId == GetCurrentTeacherId()).FirstOrDefault();
            if(dbTeacher == null )
            {
                return BadRequest("系統異常");
            }
            dbTeacher.FTeacherName = teacher.FTeacherName;
            dbTeacher.FIntroduction = teacher.FIntroduction;
            dbTeacher.FContactInfo = teacher.FContactInfo;
            dbTeacher.FNote = teacher.FNote;
            teacher = dbTeacher as TTeacher;
            // ======== 局部更新資料 end =========
            if (ModelState.IsValid)
            {
                _context.Update(teacher);
                await _context.SaveChangesAsync();
                //重導到首頁
                return RedirectToAction("TBasicInfo");
            }
            return View(teacher);
        }
        /////////////////////////////////////// ///////////////////////////////////////


        // POST: TeacherAdmin/TAddtrimage
        //動作簡述：新增單/多張圖片
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TAddtrimage(TTeacherImage trimg)
        {
            //byte[] fileBytes = await ConvertFileToByteArrayAsync(file);
            //trimg.FImageLink = fileBytes;
            //trimg.FImageLink = await ConvertFileToByteArrayAsync(file);
            trimg.FTeacherId = GetCurrentTeacherId();
            trimg.FImageSize = null;
            if (trimg.FImageName == null) { trimg.FImageName = "尚未命名"; }
            if (ModelState.IsValid)
            {
                foreach (var file in Request.Form.Files)
                {
                    if (file != null && file.Length > 0)
                    {
                        // 創建新的 TTeacherImage 對象
                        var newTrimg = new TTeacherImage
                        {
                            FTeacherId = trimg.FTeacherId,
                            FImageSize = trimg.FImageSize,
                            FImageName = trimg.FImageName
                            // 在這裡添加其他屬性的設置
                        };

                        using (BinaryReader br = new BinaryReader(file.OpenReadStream()))
                        {
                            newTrimg.FImageLink = br.ReadBytes((int)file.Length);
                        }

                        //await ReadUploadImage(newTrimg);
                        _context.Add(newTrimg);
                        await _context.SaveChangesAsync();
                    }
                }
                return RedirectToAction("TRelatedPic");
            }
            return Content("新增失敗");
        }
     
       
        // GET: TeacherAdmin/TRelatedPic
        //動作簡述：回傳老師相關圖片
        [HttpGet]
        public IActionResult TRelatedPic()
        {
            int id = GetCurrentTeacherId();
            IEnumerable<TeacherBasicViewModel> vBasicVMCollection = new List<TeacherBasicViewModel>(
                _context.TTeacherImages
                  .Where(t => t.FTeacherId == id)
                .Select(t => new TeacherBasicViewModel
                {
                    TeacherImagesId = t.FTeacherImagesId,
                    TeacherId = id,
                    ImageName = t.FImageName,
                    ImageLink = t.FImageLink,
                    Category = t.FCategory,
                    TeacherImageModel = t,
                })
            );
            return View("TRelatedPic",vBasicVMCollection);
        }
        // GET: /TeacherAdmin/EditPartialViewInfo
        //動作簡述：
        public IActionResult EditPartialViewInfo(int teacherImagesId)
        {
            var a = _context.TTeacherImages
                .Where(a => a.FTeacherImagesId == teacherImagesId)
                .Select(a => new { ImageName = a.FImageName, Category = a.FCategory })
                .FirstOrDefault();
            return Json(a);
        }
        // GET: /TeacherAdmin/GetPicture/1
        //方法簡述：讀取資料庫的圖片
        public async Task<FileResult> GetPicture(int id)
        {
            //可能沒拿到值//抓的到有值 抓不到空值
            TTeacherImage? c = await _context.TTeacherImages.FindAsync(id);
            byte[]? Content = c?.FImageLink;
            return File(Content, "image/jpeg");
        }
       
        // POST: TeacherAdmin/TdeletePic
        //動作簡述：刪除老師相關圖片(單張)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TdeletePic(int id)
        {
            var dimage = await _context.TTeacherImages.FindAsync(id);
            if (dimage != null)
            {
                _context.TTeacherImages.Remove(dimage);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(TRelatedPic));
        }
        // GET: TeacherAdmin/TEdit
        //動作簡述：產生編輯modal畫面
        public async Task<IActionResult> TEdit(int? id)
        {
            var teacherimage = await _context.TTeacherImages.FindAsync(id);
            return PartialView("T_EditTrPicPartial", teacherimage);
        }
        public async Task<IActionResult> TeditPic(TTeacherImage trimg)
        {
            trimg.FTeacherId = GetCurrentTeacherId();
            trimg.FImageSize = null;
            if (trimg.FImageName == null) { trimg.FImageName = "尚未命名"; }
            //驗證成功
            if (ModelState.IsValid)
            {
                await ReadUploadImage(trimg);
                _context.Update(trimg);
                await _context.SaveChangesAsync();
                return RedirectToAction("TRelatedPic");
            }
            return Content("修改失敗");
        }
        //方法簡述：處理上傳的圖片
        private async Task ReadUploadImage(TTeacherImage image)
        {
            //因為try是捕捉傳到資料庫的錯誤，不是捕捉讀檔案的錯誤，所以寫到try裡外都可以
            if (Request.Form.Files["FImageLink"] != null)
            //如果有值
            {
                //建立物件放在using裡面，碰到又大括號後自動回收
                //OpenReadStream把檔案(Request.Form.Files["Picture"])開啟讀取串流
                using (BinaryReader br = new BinaryReader(Request.Form.Files["FImageLink"].OpenReadStream()))
                {
                    //把BinaryReader讀到的東西，放入Entity的參數:FImageLink
                    //ReadBytes 需要的參數是int //Length:檔案長度(型態是long)
                    image.FImageLink = br.ReadBytes((int)Request.Form.Files["FImageLink"].Length);
                }
            }
            else
            //如果沒值//如果user沒有換圖
            {
                return;
                //讀原來的FImageLink
                TTeacherImage c = await _context.TTeacherImages.FindAsync(image.FTeacherImagesId);
                image.FImageLink = c.FImageLink;
                //c要解除追蹤//因為不能重複追蹤
                _context.Entry(c).State = EntityState.Detached;
            }
        }

        // /////////////////////未使用的動作及方法/////////////////////////////////
        private async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
        public IActionResult uploadimage(IFormFile file)
        {
            var fileDic = "Files";
            string filePath = Path.Combine(_hostEnv.WebRootPath, fileDic);
            if (!Directory.Exists(filePath)) { Directory.CreateDirectory(filePath); }
            var fileName = file.FileName;
            filePath = Path.Combine(filePath, fileName);
            using (FileStream fs = System.IO.File.Create(filePath))
            {
                file.CopyTo(fs);
            }
            var responseData = new
            {
                success = true,
                message = "檔案上傳成功",
                fileName = fileName,
                filePath = filePath
                // 其他您需要傳遞的資訊
            };

            // 回傳 JSON 資料
            return Json(responseData);
            //return RedirectToAction("TrelatedPic");
        }
        public byte[] ConvertIFormFileToByteArray(IFormFile file)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                file.CopyTo(ms);
                return ms.ToArray();
            }
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


    }
}
