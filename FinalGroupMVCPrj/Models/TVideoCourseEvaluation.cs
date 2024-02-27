﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FinalGroupMVCPrj.Models;

[Table("tVideoCourseEvaluation")]
public partial class TVideoCourseEvaluation
{
    [Key]
    [Column("fVideoCourseEval_Id")]
    public int FVideoCourseEvalId { get; set; }

    [Column("fMemberId")]
    public int FMemberId { get; set; }

    [Column("fVideoCourseId")]
    public int FVideoCourseId { get; set; }

    [Column("fScore")]
    public int FScore { get; set; }

    [Column("fComment")]
    [StringLength(200)]
    public string FComment { get; set; }

    [Column("fCommentDate", TypeName = "datetime")]
    public DateTime FCommentDate { get; set; }

    [Column("fCommentUpdateDate", TypeName = "datetime")]
    public DateTime? FCommentUpdateDate { get; set; }

    [Column("fDisplayStatus")]
    public bool FDisplayStatus { get; set; }
}