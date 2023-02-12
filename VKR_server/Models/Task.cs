using System;
using System.Collections.Generic;

namespace VKR_server.Models;

public partial class Task
{
    public Guid Id { get; set; }

    public string TaskText { get; set; } = null!;

    public string TaskAnswer { get; set; } = null!;

    public virtual ICollection<StudentAnswerCheck> StudentAnswerChecks { get; } = new List<StudentAnswerCheck>();

    public virtual ICollection<TasksPackTask> TasksPackTasks { get; } = new List<TasksPackTask>();
}
