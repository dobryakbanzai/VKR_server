using System;
using System.Collections.Generic;

namespace VKR_server.Models;

public partial class TasksPackTask
{
    public Guid Id { get; set; }

    public Guid TaskId { get; set; }

    public Guid PackId { get; set; }

    public virtual TasksPack Pack { get; set; } = null!;

    public virtual Task Task { get; set; } = null!;
}
