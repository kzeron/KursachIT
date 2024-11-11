using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursachIT.DataFolder
{
    public partial class ITAdminEntities : DbContext
    {
        private static ITAdminEntities context;
        public static ITAdminEntities GetContext()
        {
            if (context == null)
                context = new ITAdminEntities();
            return context;
        }
    }
}
