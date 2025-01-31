using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursachIT.ClassFolder
{
    internal class ClassHistory
    {
        public int IdHistory {  get; set; }
        public string TableName { get; set; }
        public string OperationType {  get; set; }
        public DateTime OperationTime { get; set; }
        public int? IdEmployer { get; set; }
        public string NameEmployer { get; set; }
        public string ChangedData { get; set; }
    }
}
