using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    class Patient
    {
        private string id;
        private string name;
        private string address;
        private string contactNumber;
        private string gender;
        private int age;
        private double height;
        private double weight;
        private List<string> diseasesInPast = new List<string>();
        private string otherImpInformation;
        private List<string> instructionGiven = new List<string>();

        public string Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Address { get => address; set => address = value; }
        public string ContactNumber { get => contactNumber; set => contactNumber = value; }
        public string Gender { get => gender; set => gender = value; }
        public int Age { get => age; set => age = value; }
        public double Height { get => height; set => height = value; }
        public double Weight { get => weight; set => weight = value; }
        public List<string> DiseasesInPast { get => diseasesInPast; set => diseasesInPast = value; }
        public string OtherImpInformation { get => otherImpInformation; set => otherImpInformation = value; }
        public List<string> InstructionGiven { get => instructionGiven; set => instructionGiven = value; }

        public Patient()
        {
            
        }



    }
}
