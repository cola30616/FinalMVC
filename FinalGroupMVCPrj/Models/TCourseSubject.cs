﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FinalGroupMVCPrj.Models;

[Table("tCourseSubjects")]
public partial class TCourseSubject
{
    [Column("fField_Id")]
    public int FFieldId { get; set; }

    [Key]
    [Column("fSubjectId")]
    public int FSubjectId { get; set; }

    [Required]
    [Column("fSubjectName")]
    [StringLength(50)]
    public string FSubjectName { get; set; }

    [Required]
    [Column("fSubjectCode")]
    [StringLength(50)]
    [Unicode(false)]
    public string FSubjectCode { get; set; }

    [ForeignKey("FFieldId")]
    [InverseProperty("TCourseSubjects")]
    public virtual TCourseField FField { get; set; }

    [InverseProperty("FSubject")]
    public virtual ICollection<TTeacherSubject> TTeacherSubjects { get; set; } = new List<TTeacherSubject>();
}