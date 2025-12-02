using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Management_System.Models
{
    public class UserAccount
    {
        string _username;
        string _password;
        string _role;
        int _docOrStaffId;
        string departmentName;
        string _email;
        public string Username { get => _username; set => _username = value; }
        public string Password { get => _password; set => _password = value; }
        public string Role { get => _role; set => _role = value; }
        public int DocOrStaffId { get => _docOrStaffId; set => _docOrStaffId = value; }
        public string Email { get => _email; set => _email = value; }
        public string DepartmentName { get => departmentName; set => departmentName = value; }

    }
}
