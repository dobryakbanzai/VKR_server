using System;
using System.Collections.Generic;

namespace VKR_server.Models;

public partial class TasksPack
{
    public Guid Id { get; set; }

    public string ThemeName { get; set; } = null!;

    public Guid TeacherId { get; set; }

    public virtual ICollection<PackCheck> PackChecks { get; } = new List<PackCheck>();

    public virtual ICollection<TasksPackTask> TasksPackTasks { get; } = new List<TasksPackTask>();

    public virtual Teacher Teacher { get; set; } = null!;
}
