﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FinalGroupMVCPrj.Models;

public partial class TMemberLoginLog
{
    public int FLoginLogId { get; set; }

    public int FMemberId { get; set; }

    public DateTime FLoginDateTime { get; set; }

    public string FLoginIp { get; set; }

    public string FLoginDevice { get; set; }

    public virtual TMember FMember { get; set; }
}