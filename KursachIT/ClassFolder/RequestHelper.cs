using KursachIT.DataFolder;
using System;
using System.Linq;

namespace KursachIT.ClassFolder
{
    internal class RequestHelper
    {
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
            InProgress = 6
        }
    }
}
