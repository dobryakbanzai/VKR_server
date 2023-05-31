using System;
using System.Collections.Generic;

namespace VKR_server.Models;

public partial class Challange
{
    public Guid Id { get; set; }

    public string ChallangeName { get; set; } = null!;

    public string ChallangeType { get; set; }

    public int ChallangeTarget { get; set; }

    public virtual ICollection<ChallangeStudent> ChallangeStudents { get; } = new List<ChallangeStudent>();
   
}
