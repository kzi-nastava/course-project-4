using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Repository;
using Hospital.Model;

namespace Hospital.Service
{
    class DoctorSurveyService
    {
        private DoctorSurveyRepository _doctorServiceRepository;
        private List<DoctorSurvey> _evaluatedDoctors;

        public List<DoctorSurvey> EvaluatedDoctors { get { return _evaluatedDoctors; } }

        public DoctorSurveyService()
        {
            _doctorServiceRepository = new DoctorSurveyRepository();
            _evaluatedDoctors = _doctorServiceRepository.Load();
        }
    }
}
