using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalGroupMVCPrj.Models.Metadatas
{
    public class TMemberMetadata
    {
        [Key]
        [Column("fMemberId")]
        public int FMemberId { get; set; }

        [Column("fRegisterDatetime", TypeName = "datetime")]
        public DateTime FRegisterDatetime { get; set; }

        [Required(ErrorMessage = "未填寫真實姓名")]
        [Column("fRealName")]
        [StringLength(50)]
        public string FRealName { get; set; }

        [Required]
        [Column("fShowName")]
        [StringLength(50)]
        public string FShowName { get; set; }

        [Required(ErrorMessage ="未填寫信箱")]
        [Column("fEmail")]
        [StringLength(50)]
        [EmailAddress(ErrorMessage ="信箱格式不正確")]
        public string FEmail { get; set; }

        [Column("fPhone")]
        [StringLength(10)]
        [Unicode(false)]
        public string FPhone { get; set; }

        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$",ErrorMessage = "至少至少8個字英數混合")]
        [Required(ErrorMessage ="未填寫密碼")]
        [Column("fPassword")]
        [StringLength(200)]
        public string FPassword { get; set; }

        [Column("fEmailVerification")]
        public bool FEmailVerification { get; set; }

        [Column("fMemberProfilePic")]
        public byte[] FMemberProfilePic { get; set; }

        [Column("fGetCampaignInfo")]
        public bool FGetCampaignInfo { get; set; }

        [Column("fQualifiedTeacher")]
        public bool FQualifiedTeacher { get; set; }

        [Column("fGender")]
        public bool? FGender { get; set; }

        [Column("fBirthDate", TypeName = "date")]
        public DateTime? FBirthDate { get; set; }

        [Column("fJob")]
        [StringLength(50)]
        public string FJob { get; set; }

        [Column("fEducation")]
        [StringLength(50)]
        public string FEducation { get; set; }

        [Column("fNote")]
        [StringLength(50)]
        public string FNote { get; set; }

        [Column("fStatus")]
        public bool? FStatus { get; set; }
    }
}
