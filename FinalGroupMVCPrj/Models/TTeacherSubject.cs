﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FinalGroupMVCPrj.Models;

public partial class TTeacherSubject
{
    public int FTeacherSujectsId { get; set; }

    public int FTeacherId { get; set; }

    public int FSubjectId { get; set; }

    public virtual TCourseSubject FSubject { get; set; }

    public virtual TTeacher FTeacher { get; set; }
}