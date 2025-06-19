using System;
using System.Collections.Generic;

namespace Toran.Models;

public class ToranModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateTime? CreatedAt { get; set; }
}
