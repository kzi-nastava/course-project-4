using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Model
{
    public class DoctorUser : User
    {
        public enum Speciality
        {
            Cardiologist = 1,
            Neurologist = 2,
            Psychologist = 3,
            Pediatrician = 4,
            Surgeon = 5,
            General = 6
        }

        private Speciality _speciality;

        public DoctorUser(Role role, string email, string password, string name, string surname, State state, Speciality speciality)
            : base(role, email, password, name, surname, state)
        {
            this._speciality = speciality;
        }

        public Speciality SpecialityDoctor { get { return _speciality; }set { _speciality = value; } }

        public string SpecialityToString()
		{
            if (SpecialityDoctor == Speciality.Cardiologist)
            {
                return "Kardiologija";
            }
            else if (SpecialityDoctor == Speciality.Neurologist)
            {
                return "Neurologija";
            }
            else if (SpecialityDoctor == Speciality.Pediatrician)
            {
                return "Pedijatrija";
            }
            else if (SpecialityDoctor == Speciality.Psychologist)
            {
                return "Psihologija";
            }
            else if (SpecialityDoctor == Speciality.General)
            {
                return "Opsta praksa";
            }
            return "Hirurgija";
        }
    }
}
