using System;
using System.Collections.Generic;

namespace sotrudnik.Model;

public partial class Employee
{
    static Employee emp;
    public static Employee CreateEmployee(string Surname, string Name, string Patronymic, int TitleId)
    {
        emp = new Employee();
        emp.Surname = Surname;
        emp.Name = Name;
        emp.Patronymic = Patronymic;
        emp.TitleId = TitleId;
        emp.Telephone = "не задано";
        emp.Email = "не задано";
        return emp;
    }
    public int EmployeeId { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public DateOnly? BirthDate { get; set; }

    public string? Telephone { get; set; }

    public string Email { get; set; } = null!;

    public int TitleId { get; set; }

    public virtual Title Title { get; set; } = null!;
}
