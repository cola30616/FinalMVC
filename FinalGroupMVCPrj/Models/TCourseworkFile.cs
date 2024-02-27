﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FinalGroupMVCPrj.Models;

[Table("tCourseworkFile")]
public partial class TCourseworkFile
{
    [Key]
    [Column("fCourseworkFileId")]
    public int FCourseworkFileId { get; set; }

    [Column("fCourseworkId")]
    public int FCourseworkId { get; set; }

    [Required]
    [Column("fFileName")]
    [StringLength(50)]
    public string FFileName { get; set; }

    [Column("fFileFormat")]
    [StringLength(50)]
    public string FFileFormat { get; set; }

    [Column("fFileLink")]
    public string FFileLink { get; set; }

    [Column("fFileSize")]
    public int FFileSize { get; set; }

    [ForeignKey("FCourseworkId")]
    [InverseProperty("TCourseworkFiles")]
    public virtual TCoursework FCoursework { get; set; }
}