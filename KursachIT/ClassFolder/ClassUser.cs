using KursachIT.DataFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursachIT.ClassFolder
{
    public class ClassUser
    {
        public int IdUser { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public int IdOffice { get; set; }
        public string NameOffice { get; set; }
        public int NumberOffice { get; set; }
        public string NumberPhone { get; set; }
        public string Email {  get; set; } 
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; }
        public int IdRole {  get; set; }
    }
    public enum Role
    {
        Admin,
        User, 
        Technician
    }
    public static class AuthUser
    {
        public static int IdCurretUser { get; set; }
    }
    public static class SharedData
    {
        public static int LastUserId { get; set; }
    }
}
