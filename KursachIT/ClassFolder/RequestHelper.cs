using KursachIT.DataFolder;
using System;
using System.Data.Entity;
using System.Linq;

namespace KursachIT.ClassFolder
{
    internal class RequestHelper
    {
        public static void AssignExecutor(int currentUser, ClassRequest request)
        {
            using(var context = new ITAdminEntities())
            {
                // Находим сотрудника с таким же Id, как у пользователя
                var matchingEmployer = context.Employers
                    .FirstOrDefault(emp => emp.IdUser == currentUser);

                if (matchingEmployer != null)
                {
                    // Устанавливаем IdExcutor в запросе
                    request.IdExcutor = matchingEmployer.IdEmployers;
                }
                else
                {
                    throw new Exception("Исполнитель не найден для текущего пользователя.");
                }
            }
            
        }

        public static void UpdateOverdueRequests()
        {
            using (var context = new ITAdminEntities())
            {
                DateTime currentDate = DateTime.Now;

                var overdueRequests = context.Requests
                    .Where(r => r.PlanDate < currentDate &&
                                r.IdStatus != (int)StatusEnum.Completed &&
                                r.IdStatus != (int)StatusEnum.Denied &&
                                r.IdStatus != (int)StatusEnum.Canceled )
                    .ToList();

                foreach (var request in overdueRequests)
                {
                    request.IdStatus = 5;
                }

                context.SaveChanges();
            }
        }
        public enum StatusEnum
        {
                New = 1,
                Completed = 2,
                Denied = 3,
                Canceled = 4,
                Overdue = 5,
                InProgress = 6,
                Worked = 7,
                Deleted = 8

        }
    }
}
