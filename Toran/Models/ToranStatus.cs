using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Toran.Models;

[Table("ToranStatus")]
public partial class ToranStatus
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string EmployeeName { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? LastDutyDate { get; set; }
}
