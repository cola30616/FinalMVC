using FinalGroupMVCPrj.Models;
using FinalGroupMVCPrj.Models.DTO;
using FinalGroupMVCPrj.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Pkcs;

namespace FinalGroupMVCPrj.Controllers
{
    [AllowAnonymous]
    public class TeacherController : UserInfoController
    {
        private readonly LifeShareLearnContext _context;  //資料庫
        public TeacherController(LifeShareLearnContext context)
        {
            _context = context;
        }

        // GET: Teacher/List
        //動作簡述：回傳老師清單的頁面
        [HttpGet]
        public IActionResult List()
        {
            return View();
        }

        // GET: Teacher/Info
        //動作簡述：回傳單一老師資訊的頁面
        [HttpGet]
        public IActionResult Info(int id)
        {
            IEnumerable<TeacherBasicViewModel> tBasicVMCollection = new List<TeacherBasicViewModel>(
                _context.TTeachers
                .Include(t => t.TTeacherSubjects)
                .ThenInclude(t => t.FSubject)
                .Where(t => t.FTeacherId == id)
                .Select(t => new TeacherBasicViewModel
                {
                    TeacherId = t.FTeacherId,
                    TeacherName = t.FTeacherName,
                    TeacherProfilePicURL = (t.FTeacherProfilePic != null) ? GetImageDataURL(t.FTeacherProfilePic) : "https://upload.wikimedia.org/wikipedia/commons/thumb/6/65/No-Image-Placeholder.svg/1665px-No-Image-Placeholder.svg.png",
                    Introduction = t.FIntroduction,
                    ContactInfo = t.FContactInfo,
                    Note = t.FNote,
                    SubjectName = t.TTeacherSubjects.Select(ts => ts.FSubject.FSubjectName),
                })
                );
            ViewBag.Id = id;
            //var tr = _context.TTeachers.FindAsync(id);
            return View("Info", tBasicVMCollection);
        }


        // ============== apicontroller ============== // 
        // GET: Teacher/OneTrInfo
        //動作簡述：在info.cshtml的老師履歷多圖使用
        [HttpPost]
        public IActionResult OneTrInfo([FromBody] TeacherListDTO _search,int id)
        {
            var ti = _context.TTeacherImages
                .Where(t => t.FTeacherId == id)
                .Select(t => new
            {
                t.FTeacherId,
                t.FImageName,
                t.FCategory,
                FImageLinkURL = GetImageDataURL(t.FImageLink),
            });
            //根據老師多圖的科目fCategory搜尋
            if (!string.IsNullOrEmpty(_search.Keywordca))
            {
                if (_search.Keywordca == "全部")
                {
                    ti = ti.Where(t => t.FCategory.Contains("教育背景") || t.FCategory.Contains("證照證書") || t.FCategory.Contains("作品") || t.FCategory == null);
                }
                else if (_search.Keywordca == "其他")
                {
                    ti = ti.Where(t => t.FCategory==null);
                }
                else
                {
                    ti = ti.Where(t => t.FCategory.Contains(_search.Keywordca));
                }
            }
            //總共有幾筆
            int totalCount = ti.Count();
            //一頁幾筆資料
            int pageSize = _search.PageSize ?? 3;
            //計算總共有幾頁
            int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            //目前第幾頁
            int page = _search.Page ?? 1;

            //分頁
            ti = ti.Skip((page - 1) * pageSize).Take(pageSize);

            TeachersPagingDTO cardsPaging = new TeachersPagingDTO();
            cardsPaging.TotalPages = totalPages;
            List<Models.DTO.TTeacherImage> teacherInfos = ti.Select(t => new Models.DTO.TTeacherImage
            {
                FImageName = t.FImageName,
                FCategory = t.FCategory,
                TeacherImagesURL = t.FImageLinkURL,
                FTeacherId = t.FTeacherId,
            }).ToList();
            cardsPaging.CategoriesResult = teacherInfos;

            return Json(cardsPaging);
        }
        // GET: Teacher/AllTrInfo
        //動作簡述：在List.cshtml的老師卡片使用中
        [HttpPost]
        public IActionResult AllTrInfo([FromBody] TeacherListDTO _search)
        {
            var tr = _context.TTeachers
                .Include(t => t.TTeacherSubjects)
                .ThenInclude(t => t.FSubject)
            .Select(t => new
            {
                t.FTeacherId,
                t.FTeacherName,
                TeacherProfilePicURL = (t.FTeacherProfilePic != null) ? GetImageDataURL(t.FTeacherProfilePic) : "https://upload.wikimedia.org/wikipedia/commons/thumb/6/65/No-Image-Placeholder.svg/1665px-No-Image-Placeholder.svg.png",
                SubjectNames = t.TTeacherSubjects.Select(ts => ts.FSubject.FSubjectName)
                //SubjectName = t.TTeacherSubjects.Select(ts => ts.FSubject).Select(t => new { t.FSubjectName })
            });
            //根據老師名稱搜尋
            if (!string.IsNullOrEmpty(_search.Keywordtn))
            {
                tr = tr.Where(t => t.FTeacherName.Contains(_search.Keywordtn));
            }
            //根據科目搜尋
            if (!string.IsNullOrEmpty(_search.Keywordsb))
            {
                if (_search.Keywordsb == "所有老師")
                {
                    tr = tr.Select(t => t);
                }
                else
                {
                    tr = tr.Where(t => t.SubjectNames.Contains(_search.Keywordsb));
                }
            }
            //總共有幾筆
            int totalCount = tr.Count();
            //一頁幾筆資料
            int pageSize = _search.PageSize ?? 3;
            //計算總共有幾頁
            int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            //目前第幾頁
            int page = _search.Page ?? 1;

            //分頁
            tr = tr.Skip((page - 1) * pageSize).Take(pageSize);

            TeachersPagingDTO cardsPaging = new TeachersPagingDTO();
            cardsPaging.TotalPages = totalPages;
            List<TeacherInfo> teacherInfos = tr.Select(t => new TeacherInfo
            {
                FTeacherId = t.FTeacherId,
                FTeacherName = t.FTeacherName,
                TeacherProfilePicURL = t.TeacherProfilePicURL,
                SubjectNames = (List<string>)t.SubjectNames
            }).ToList();
            cardsPaging.CardsResult = teacherInfos;
            return Json(cardsPaging);
        }

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

        // GET: Teacher/AllTeachersPhotos
        //動作簡述：回傳所有老師頭像的url(沒有使用到)
        [HttpGet]
        public async Task<IActionResult> AllTeachersPhotos()
        {
            List<string> allTeacherData = new List<string>();
            List<TTeacher> allTeachers = await _context.TTeachers.ToListAsync();

            foreach (var teacher in allTeachers)
            {
                byte[]? image = teacher?.FTeacherProfilePic;
                int? id = teacher?.FTeacherId;
                if (image != null && image.Length > 0)
                {
                    string base64String = Convert.ToBase64String(image);
                    string blobDataURL = $"data:image/jpeg;base64,{base64String}";
                    allTeacherData.Add((blobDataURL));
                }
                var tr = _context.TTeachers.Select(t => t.FTeacherProfilePic);
            }
            return Json(allTeacherData);
        }


        // GET: Teacher/TeacherPhoto
        //動作簡述：回傳老師頭像的url
        [HttpGet]
        public async Task<IActionResult> TeacherPhoto(int? id)
        {
            string blobDataURL = "";
            TTeacher? teacher = await _context.TTeachers.FirstOrDefaultAsync(t => t.FTeacherId == id);
            byte[]? image = teacher?.FTeacherProfilePic;
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

    //VideoPlayer，viewComponent
    public class VideoPlayer : ViewComponent
    {
        private readonly LifeShareLearnContext _context;

        public VideoPlayer(LifeShareLearnContext context)
        {
            _context = context;
        }


        // 這邊使用viewComponent
        public async Task<IViewComponentResult> InvokeAsync(int? id)
        {
            
            var video = await _context.TVideoUploadUrls.Where(u => u.FTeacherId == id).FirstOrDefaultAsync();           
            return View(video);
        }
    }
}
