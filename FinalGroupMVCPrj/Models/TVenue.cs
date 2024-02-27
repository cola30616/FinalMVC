﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FinalGroupMVCPrj.Models;

[Table("tVenue")]
public partial class TVenue
{
    [Key]
    [Column("fVenueId")]
    public int FVenueId { get; set; }

    [Column("fVenueProviderId")]
    public int FVenueProviderId { get; set; }

    [Required]
    [Column("fVenueCode")]
    [StringLength(50)]
    public string FVenueCode { get; set; }

    [Required]
    [Column("fVenueName")]
    [StringLength(50)]
    public string FVenueName { get; set; }

    [Column("fDistrictId")]
    public int FDistrictId { get; set; }

    [Required]
    [Column("fAddressDetail")]
    [StringLength(50)]
    public string FAddressDetail { get; set; }

    [Column("fAddedTime", TypeName = "date")]
    public DateTime? FAddedTime { get; set; }

    [Column("fMaxPeople")]
    public short FMaxPeople { get; set; }

    [Column("fPriceHalfHr", TypeName = "money")]
    public decimal FPriceHalfHr { get; set; }

    [Column("fOpenStatus")]
    public bool FOpenStatus { get; set; }

    [Column("fReviewStatus")]
    public bool FReviewStatus { get; set; }

    [Column("fNote")]
    [StringLength(50)]
    public string FNote { get; set; }

    [ForeignKey("FVenueProviderId")]
    [InverseProperty("TVenues")]
    public virtual TVenueProviderInfo FVenueProvider { get; set; }
}