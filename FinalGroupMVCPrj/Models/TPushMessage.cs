﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FinalGroupMVCPrj.Models;

[Table("tPushMessage")]
public partial class TPushMessage
{
    [Key]
    [Column("fPushMessageId")]
    public int FPushMessageId { get; set; }

    [Required]
    [Column("fPushType")]
    [StringLength(50)]
    public string FPushType { get; set; }

    [Column("fPushLayoutId")]
    public int FPushLayoutId { get; set; }

    [Required]
    [Column("fPushMethod")]
    [StringLength(50)]
    public string FPushMethod { get; set; }

    [Column("fPushStartDate", TypeName = "date")]
    public DateTime FPushStartDate { get; set; }

    [Column("fPushEndDate", TypeName = "date")]
    public DateTime FPushEndDate { get; set; }

    [Required]
    [Column("fPushContent")]
    public string FPushContent { get; set; }

    [Column("fPushImagePath")]
    public byte[] FPushImagePath { get; set; }

    [Column("fPushCreatedTime", TypeName = "datetime")]
    public DateTime FPushCreatedTime { get; set; }

    [Column("fPushLastUpdatedTime", TypeName = "datetime")]
    public DateTime FPushLastUpdatedTime { get; set; }

    [Column("fEmployeeId")]
    public int FEmployeeId { get; set; }

    [ForeignKey("FEmployeeId")]
    [InverseProperty("TPushMessages")]
    public virtual TAdmin FEmployee { get; set; }

    [ForeignKey("FPushLayoutId")]
    [InverseProperty("TPushMessages")]
    public virtual TPushLayout FPushLayout { get; set; }

    [InverseProperty("FPushMessage")]
    public virtual ICollection<TMemberGetPush> TMemberGetPushes { get; set; } = new List<TMemberGetPush>();
}