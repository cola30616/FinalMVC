﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FinalGroupMVCPrj.Models.DTO
{
public class TChatMessageDTO
{
    public int FMessageId { get; set; }

    public int? FChatRoomId { get; set; }

    public int FTeacherId { get; set; }

    public int FMemberId { get; set; }

    public string FMessage { get; set; }

    public DateTime FMessageTime { get; set; }

    public bool FIsTeacherMsg { get; set; }

    public bool isRead { get; set; }
}
}