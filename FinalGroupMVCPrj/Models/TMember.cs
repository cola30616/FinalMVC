﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FinalGroupMVCPrj.Models;

public partial class TMember
{
    public int FMemberId { get; set; }

    public DateTime FRegisterDatetime { get; set; }

    public string FRealName { get; set; }

    public string FShowName { get; set; }

    public string FEmail { get; set; }

    public string FPhone { get; set; }

    public string FPassword { get; set; }

    public bool FEmailVerification { get; set; }

    public byte[] FMemberProfilePic { get; set; }

    public bool FGetCampaignInfo { get; set; }

    public bool FQualifiedTeacher { get; set; }

    public bool? FGender { get; set; }

    public DateTime? FBirthDate { get; set; }

    public string FJob { get; set; }

    public string FEducation { get; set; }

    public string FNote { get; set; }

    public bool? FStatus { get; set; }

    public virtual ICollection<TChatRoomTeacher> TChatRoomTeachers { get; set; } = new List<TChatRoomTeacher>();

    public virtual ICollection<TLessonEvaluation> TLessonEvaluations { get; set; } = new List<TLessonEvaluation>();

    public virtual ICollection<TMemberCitiesList> TMemberCitiesLists { get; set; } = new List<TMemberCitiesList>();

    public virtual ICollection<TMemberFavCourse> TMemberFavCourses { get; set; } = new List<TMemberFavCourse>();

    public virtual ICollection<TMemberFavTeacher> TMemberFavTeachers { get; set; } = new List<TMemberFavTeacher>();

    public virtual ICollection<TMemberLoginLog> TMemberLoginLogs { get; set; } = new List<TMemberLoginLog>();

    public virtual ICollection<TMemberWishField> TMemberWishFields { get; set; } = new List<TMemberWishField>();

    public virtual ICollection<TOrder> TOrders { get; set; } = new List<TOrder>();

    public virtual ICollection<TTeacherApplyLog> TTeacherApplyLogs { get; set; } = new List<TTeacherApplyLog>();

    public virtual ICollection<TTeacher> TTeachers { get; set; } = new List<TTeacher>();
}