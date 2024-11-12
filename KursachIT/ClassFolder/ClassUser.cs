using KursachIT.DataFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursachIT.ClassFolder
{
    internal class ClassUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; }
    }
    public enum Role
    {
        Admin,
        User, 
        Technician
    }

}
