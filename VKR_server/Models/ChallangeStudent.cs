using System;
using System.Collections.Generic;

namespace VKR_server.Models;

public partial class ChallangeStudent
{
   

    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public Guid ChallangeId { get; set; }
    /*
   public virtual Challange Challange { get; set; } = null;

   public virtual Student Student { get; set; } = null;*/
}
