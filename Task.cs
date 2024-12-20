using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Manager
{
    public class Task
    {
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public string PriorityLevel { get; set; }
        public string DueDate { get; set; }
    }
}
