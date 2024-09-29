using System;
using System.Collections.Generic;

namespace sotrudnik.Model;

public partial class Title
{
    public int TitleId { get; set; }

    public string TitleName { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
