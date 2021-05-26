using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    class DoctorTiming
    {
        private int day;
        
        private string startTime;
        private string endTime;

        public int Day { get => day; set => day = value; }
        public string StartTime { get => startTime; set => startTime = value; }
        public string EndTme { get => endTime; set => endTime = value; }

        public DoctorTiming()
        {
            
        }


    }
}
