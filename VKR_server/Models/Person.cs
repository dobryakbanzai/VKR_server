using System;
using System.Collections.Generic;

namespace VKR_server.Models;

public partial class Person
{
    public Guid Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string? PersonRole { get; set; }
}
