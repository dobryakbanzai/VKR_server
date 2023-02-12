using System;
using System.Collections.Generic;

namespace VKR_server.Models;

public partial class Teacher
{
    public Guid Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public int? Experience { get; set; }

    public string Login { get; set; } = null!;

    public string Pass { get; set; } = null!;

    public string? PersonRole { get; set; }

    public virtual ICollection<Student> Students { get; } = new List<Student>();

    public virtual ICollection<TasksPack> TasksPacks { get; } = new List<TasksPack>();
}
