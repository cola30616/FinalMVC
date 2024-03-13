﻿using System.ComponentModel.DataAnnotations;

namespace FinalGroupMVCPrj.Models.ViewModels
{
    public class CourseEvaluationViewModel
    {
        [Display(Name = "課程評價ID")]
        public int FLessonEvalId { get; set; }
        [Display(Name = "會員ID")]
        public int FMemberId { get; set; }
        [Display(Name = "會員暱稱")]
        public string FShowName { get; set; }
        [Display(Name = "訂單明細ID")]
        public int FOrderDetailId { get; set; }
        [Display(Name = "課程ID")]
        public int FLessonCourseId { get; set; }
        [Display(Name = "課程名稱")]
        public string FName { get; set; }
        [Display(Name = "評價分數")]
        public int FScore { get; set; }
        [Display(Name = "評價內容")]
        public string? FComment { get; set; }
        [Display(Name = "評價時間")]
        public DateTime FCommentDate { get; set; }
        [Display(Name = "評價狀態")]
        public bool FDisplayStatus { get; set; }
    }
}