using System;
using System.Collections.Generic;

namespace VKR_server.Models;

public partial class ChallangeType
{
    public Guid Id { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Challange> Challanges { get; } = new List<Challange>();
}
