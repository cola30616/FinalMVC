﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FinalGroupMVCPrj.Models;

[Table("tMember")]
public partial class TMember
{
    [Key]
    [Column("fMemberId")]
    public int FMemberId { get; set; }

    [Column("fRegisterDatetime", TypeName = "datetime")]
    public DateTime FRegisterDatetime { get; set; }

    [Required]
    [Column("fRealName")]
    [StringLength(50)]
    public string FRealName { get; set; }

    [Required]
    [Column("fShowName")]
    [StringLength(50)]
    public string FShowName { get; set; }

    [Required]
    [Column("fEmail")]
    [StringLength(50)]
    public string FEmail { get; set; }

    [Column("fPhone")]
    [StringLength(10)]
    [Unicode(false)]
    public string FPhone { get; set; }

    [Required]
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

    [InverseProperty("FMember")]
    public virtual ICollection<TChatRoomTeacher> TChatRoomTeachers { get; set; } = new List<TChatRoomTeacher>();

    [InverseProperty("FMember")]
    public virtual ICollection<TLessonEvaluation> TLessonEvaluations { get; set; } = new List<TLessonEvaluation>();

    [InverseProperty("FMember")]
    public virtual ICollection<TMemberCitiesList> TMemberCitiesLists { get; set; } = new List<TMemberCitiesList>();

    [InverseProperty("FMember")]
    public virtual ICollection<TMemberFavCourse> TMemberFavCourses { get; set; } = new List<TMemberFavCourse>();

    [InverseProperty("FMember")]
    public virtual ICollection<TMemberFavTeacher> TMemberFavTeachers { get; set; } = new List<TMemberFavTeacher>();

    [InverseProperty("FMember")]
    public virtual ICollection<TMemberLoginLog> TMemberLoginLogs { get; set; } = new List<TMemberLoginLog>();

    [InverseProperty("FMember")]
    public virtual ICollection<TMemberWishField> TMemberWishFields { get; set; } = new List<TMemberWishField>();

    [InverseProperty("FMember")]
    public virtual ICollection<TOrder> TOrders { get; set; } = new List<TOrder>();

    [InverseProperty("FMember")]
    public virtual ICollection<TTeacherApplyLog> TTeacherApplyLogs { get; set; } = new List<TTeacherApplyLog>();

    [InverseProperty("FMember")]
    public virtual ICollection<TTeacher> TTeachers { get; set; } = new List<TTeacher>();
}