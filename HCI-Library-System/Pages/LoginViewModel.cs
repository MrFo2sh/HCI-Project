using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HCI_Library_System.Pages
{
    class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UsernameRule : ValidationRule
    {
        public UsernameRule()
        {
        }



        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string u = (string)value;
            if (u == null || u.Trim().Length == 0)
            {
                return new ValidationResult(false, "Username cannot be empty");
            }
            
            if (u.Contains('#'))
            {
                return new ValidationResult(false, "Illegal characters");
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }

    }
}
