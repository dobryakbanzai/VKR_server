using System;
using System.Collections.Generic;

namespace VKR_server.Models;

public partial class Student
{
    public Guid Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string? PersonRole { get; set; }

    public int? EdClass { get; set; }

    public string Login { get; set; } = null!;

    public string Pass { get; set; } = null!;

    public Guid? TeacherId { get; set; }

    public int? AryProg { get; set; }

    public int? DerProg { get; set; }

    public int? TasksProg { get; set; }

    public virtual Teacher? Teacher { get; set; }
}
