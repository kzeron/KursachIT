using KursachIT.DataFolder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursachIT.ClassFolder
{
    internal class ClassSaveSassion
    {
        public static void SaveSession(User user)
        {
            
            string filePath = "session.txt"; // путь к файлу
            using (StreamWriter writer = new StreamWriter(filePath, false)) // false - перезаписывать файл
            {
                writer.WriteLine($"IdUser={user.IdLogin}");
                writer.WriteLine($"Login={user.Login}");
                writer.WriteLine($"Role={user.IdRole}");
                writer.WriteLine($"SessionStart={DateTime.Now}");
            }
        }
        public static User LoadSession()
        {
            string filePath = "session.txt";
            if (!File.Exists(filePath))
            {
                return null;
            }
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                int idLogin = int.Parse(lines.FirstOrDefault(line => line.StartsWith("IdUser="))?.Split('=')[1] ?? "0");
                string login = lines.FirstOrDefault(line => line.StartsWith("Login="))?.Split('=')[1];
                int idRole = int.Parse(lines.FirstOrDefault(line => line.StartsWith("Role="))?.Split('=')[1] ?? "0");

                var user = ITAdminEntities.GetContext().User.FirstOrDefault(u => u.IdLogin == idLogin && u.Login == login);
                return user;
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB(ex);
                return null;
            }
        }
        public static void ClearSession()
        {
            string filePath = "session.txt";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }


    }
}
