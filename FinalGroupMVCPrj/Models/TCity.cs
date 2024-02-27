﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FinalGroupMVCPrj.Models;

[Table("tCity")]
public partial class TCity
{
    [Key]
    [Column("fCityId")]
    public int FCityId { get; set; }

    [Required]
    [Column("fCityName")]
    [StringLength(50)]
    public string FCityName { get; set; }

    [InverseProperty("FCity")]
    public virtual ICollection<TCityDistrict> TCityDistricts { get; set; } = new List<TCityDistrict>();

    [InverseProperty("FCity")]
    public virtual ICollection<TMemberCitiesList> TMemberCitiesLists { get; set; } = new List<TMemberCitiesList>();
}