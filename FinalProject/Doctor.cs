using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FinalProject
{
    class Doctor
    {
        private string id;
        private string name;
        private string department;
        private string specialist;
        private List<DoctorTiming> timingList;

        public string Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Department { get => department; set => department = value; }
        public string Specialist { get => specialist; set => specialist = value; }
        public List<DoctorTiming> TimingList { get => timingList; set => timingList = value; }

        public Doctor()
        {

        }


        

    }
}
