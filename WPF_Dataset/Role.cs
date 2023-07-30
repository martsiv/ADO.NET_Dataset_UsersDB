using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Dataset
{
    public class Role
    {
        public int RoleID { get; init; }
        public string RoleName { get; init; }
        public override string ToString()
        {
            return $"{RoleID}, {RoleName}";
        }
    }
}
