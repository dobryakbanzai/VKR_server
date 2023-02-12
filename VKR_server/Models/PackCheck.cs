using System;
using System.Collections.Generic;

namespace VKR_server.Models;

public partial class PackCheck
{
    public Guid Id { get; set; }

    public Guid PackId { get; set; }

    public Guid CheckId { get; set; }

    public virtual StudentAnswerCheck Check { get; set; } = null!;

    public virtual TasksPack Pack { get; set; } = null!;
}
