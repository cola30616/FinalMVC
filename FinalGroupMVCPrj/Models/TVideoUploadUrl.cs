﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FinalGroupMVCPrj.Models;

[Table("tVideoUploadUrl")]
public partial class TVideoUploadUrl
{
    [Key]
    [Column("fVideoUploadUrl_Id")]
    public int FVideoUploadUrlId { get; set; }

    [Column("fVideoCourseId")]
    public int FVideoCourseId { get; set; }

    [Column("fVideoName")]
    [StringLength(50)]
    public string FVideoName { get; set; }

    [Column("fVideoPath")]
    [StringLength(200)]
    public string FVideoPath { get; set; }

    [Column("fUploadTime", TypeName = "datetime")]
    public DateTime? FUploadTime { get; set; }
}