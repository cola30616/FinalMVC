namespace FinalGroupMVCPrj.Models.DTO
{
    public class CourseListDTO
    {
        public string Keyword { get; set; }
        public int? FieldId { get; set; }
        public int? SubjectId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public float? MinRating { get; set; }
        public float? MaxRating { get; set; }
        public string SortBy { get; set; }
        public string SortType { get; set; }

        private int _page = 1; // 將 Page 屬性改為非空的整數，並設置默認值為 1
        public int Page
        {
            get { return _page; }
            set { _page = Math.Max(value, 1); } // 確保 Page 不低於 1
        }
        public int? PageSize { get; set; } = 9;
    }
}
