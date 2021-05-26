using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject
{
    
    class Appointment
    {
        public Patient patient;
        public Doctor doctor;
        public DateTime appointmentDate;
        public Appointment(Patient p, Doctor d, DateTime dt)
        {
            patient = p;
            doctor = d;
            appointmentDate = dt;

        }
        public Appointment()
        {

        }
    }
}
