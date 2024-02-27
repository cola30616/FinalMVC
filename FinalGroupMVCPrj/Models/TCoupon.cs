﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FinalGroupMVCPrj.Models;

[Table("tCoupon")]
public partial class TCoupon
{
    [Key]
    [Column("fCouponId")]
    public int FCouponId { get; set; }

    [Required]
    [Column("fDiscountCode")]
    [StringLength(50)]
    public string FDiscountCode { get; set; }

    [Column("fCouponValidOrNot")]
    public bool FCouponValidOrNot { get; set; }

    [Column("fCouponDiscount", TypeName = "money")]
    public decimal FCouponDiscount { get; set; }

    [Column("fCouponCreatedDate", TypeName = "date")]
    public DateTime FCouponCreatedDate { get; set; }

    [Column("fCouponStartDate", TypeName = "date")]
    public DateTime FCouponStartDate { get; set; }

    [Column("fCouponEndDate", TypeName = "date")]
    public DateTime FCouponEndDate { get; set; }

    [Column("fCouponConditionId")]
    public int? FCouponConditionId { get; set; }

    [ForeignKey("FCouponConditionId")]
    [InverseProperty("TCoupons")]
    public virtual TCouponConditionList FCouponCondition { get; set; }
}