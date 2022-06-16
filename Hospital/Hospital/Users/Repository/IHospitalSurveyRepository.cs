using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Users.Model;

namespace Hospital.Users.Repository
{
    interface IHospitalSurveyRepository: IRepository<HospitalSurvey>
    {
    public interface IHospitalSurveyRepository : IRepository<HospitalSurvey>
    {
        HospitalSurvey InputValuesForServey(string patientEmail);
    }
}
