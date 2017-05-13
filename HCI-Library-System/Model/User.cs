using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI_Library_System.Model
{
    class User
    {
        public enum UserType
        {
            Student = 0,
            Teacher,
            Admin
        }
        public UserType Type { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Birthdate { get; set; }
        public string ImageUrl { get; set; }
    }
}
