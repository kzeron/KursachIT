using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursachIT.ClassFolder
{
    public class ClassRequest
    {
        public int IdRequst {  get; set; }
        public int IdStatus {  get; set; }
        public string NameStatus { get; set; }
        public string NameRequest { get; set; }
        public int IdCategory { get; set; }
        public string NameCategory { get; set; }
        public int IdPriority { get; set; }
        public string NamePriority { get; set; }
        public DateTime PlanDate { get; set; }
        public int IdRequestSender {  get; set; }
        public int IdExcutor { get; set; }
        public string Transcription { get; set; }
        public string NameExcutor { get; set; }
        public string NameRequestSender { get; set; }
        public DateTime DateRealize { get; set; }
    }
}
