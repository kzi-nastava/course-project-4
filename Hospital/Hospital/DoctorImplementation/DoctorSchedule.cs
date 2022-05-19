using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Service;

namespace Hospital.DoctorImplementation
{
    class DoctorSchedule
    {
        List<Appointment> allMyAppointments;
        User currentRegisteredDoctor;
        List<HealthRecord> healthRecords;
        AppointmentService appointmentService;


        public DoctorSchedule(AppointmentService serviceAppointment, List<HealthRecord> allHealthRecords, List<Appointment> appointments, User doctor)
        {
            appointmentService = serviceAppointment;
            healthRecords = allHealthRecords;
            allMyAppointments = appointments;
            currentRegisteredDoctor = doctor;


        }

        public void ReadOwnAppointmentSpecificDate(DateTime dateSchedule)
        {
            DateTime dateForNextThreeDays = dateSchedule.AddDays(3);
            List<Appointment> appointmentsOfParticularDay = AppointmentForSelectedDate(dateSchedule, dateForNextThreeDays);
            Console.WriteLine("Raspored za datum od " + dateSchedule.Month + "/" + dateSchedule.Day + "/" + dateSchedule.Year + " do " + dateForNextThreeDays.Month + "/" + dateForNextThreeDays.Day + "/" + dateForNextThreeDays.Year);
            if (appointmentsOfParticularDay.Count != 0)
            {
                int serialNumberAppointment = 1;
                Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,10}|{3,10}|{4,10}|{5,10}|{6,10}", "Br.", "Pacijent", "Datum", "Pocetak", "Kraj", "Soba", "Vrsta termina"));
                foreach (Appointment appointmentOwn in appointmentsOfParticularDay)
                {

                    Console.WriteLine(appointmentOwn.ToStringDisplayForDoctor(serialNumberAppointment));
                    serialNumberAppointment += 1;

                }
                this.AccessToHealthRecord(appointmentsOfParticularDay);
            }
            else
            {
                Console.WriteLine("Nemate zakazanih termina za izabrani datum");
            }
            
        }

        private List<Appointment> AppointmentForSelectedDate(DateTime selectedDate, DateTime dateForNextThreeDays)
        {
            List<Appointment> appointmentsOfParticularDay = new List<Appointment>();
            foreach (Appointment appointmentOwn in this.allMyAppointments)
            {
                if (appointmentOwn.DoctorEmail.Equals(currentRegisteredDoctor.Email) && (appointmentOwn.DateAppointment >= selectedDate) && (appointmentOwn.DateAppointment <= dateForNextThreeDays))
                {
                    appointmentsOfParticularDay.Add(appointmentOwn);
                }
            }
            return appointmentsOfParticularDay;
        }

        private void AccessToHealthRecord(List<Appointment> appointmentsOfParticularDay)
        {
            string choice, serialNumberOfHealthRecord;
            Appointment appointmentOfSelectedPatient;
            do
            {
                Console.WriteLine("Da li želite da pristupite zdravstvenom kartonu od nekog pacijenta? \n1) DA\n2) NE\nUnesite 1 ili 2.");
                choice = Console.ReadLine();
                if (choice.Equals("1"))
                {
                    do
                    {
                        do
                        {
                            Console.WriteLine("Unesite redni broj pacijenta čiji zdravstveni karton želite da pogledate: ");
                            serialNumberOfHealthRecord = Console.ReadLine();

                        } while (!appointmentService.IsIntegerValid(serialNumberOfHealthRecord));
                    } while (Int32.Parse(serialNumberOfHealthRecord) > appointmentsOfParticularDay.Count);
                    appointmentOfSelectedPatient = appointmentsOfParticularDay[Int32.Parse(serialNumberOfHealthRecord) - 1];
                    this.PrintPatientsHealthRecord(appointmentOfSelectedPatient.PatientEmail);
                }
                else if (choice.Equals("2"))
                {
                    return;
                }
            } while (!choice.Equals("1") && !choice.Equals("2"));

        }

        private void PrintPatientsHealthRecord(String patientEmail)
        {
            foreach (HealthRecord healthRecord in this.healthRecords)
            {
                if (healthRecord.EmailPatient.Equals(patientEmail))
                {
                    Console.WriteLine(healthRecord.ToString());
                }
            }

        }
    }
}
