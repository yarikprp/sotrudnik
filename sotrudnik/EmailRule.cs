using System;
using System.Globalization;
using System.Windows.Controls;

namespace sotrudnik.ValidationRules
{
    public class EmailRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string email = string.Empty;

            if (value != null)
            {
                email = value.ToString();
            }
            else
            {
                return new ValidationResult(false, "Адрес электронной почты не задан!");
            }

            if (email.Contains("@") && email.Contains("."))
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false,
                    "Адрес электронной почты должен содержать символы @ и (.) точки \n" +
                    "Шаблон адреса: adres@mymail.com");
            }
        }
    }
}