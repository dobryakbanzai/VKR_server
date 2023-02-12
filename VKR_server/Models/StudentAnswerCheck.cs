using System;
using System.Collections.Generic;

namespace VKR_server.Models;

public partial class StudentAnswerCheck
{
    public Guid Id { get; set; }

    public Guid TaskId { get; set; }

    public Guid StudentId { get; set; }

    public string? StudentAnswer { get; set; }

    public bool AnswerCorr { get; set; }

    public virtual ICollection<PackCheck> PackChecks { get; } = new List<PackCheck>();

    public virtual Student Student { get; set; } = null!;

    public virtual Task Task { get; set; } = null!;
}
