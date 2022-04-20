using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Repository;

namespace Hospital.Service
{
    class AppointmentService
    {
        private AppointmentRepository appointmentRepository;
        private List<Appointment> appointments;

        public AppointmentService()
        {
            appointmentRepository = new AppointmentRepository();
            appointments = appointmentRepository.Load();
        }
    }
}
