using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Dataset
{
    public class User
    {
        private string name;
        private string password;
        private string address;
        private string phone;
        private Role role = null;
        public string Name { get { return name; } set { name = value; } }
        public string Password { get { return password; } set { password = value; } }
        public string Address { get { return address; } set { address = value; } }
        public string Phone { get { return phone; } set { phone = value; } }
        public Role Role { get { return role; } set { role = value; } }
        public User() { }
    }
}
