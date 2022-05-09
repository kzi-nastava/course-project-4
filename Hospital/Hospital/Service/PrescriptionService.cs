using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Model;
using Hospital.Repository;

namespace Hospital.Service
{
    class PrescriptionService
    {
        private PrescriptionRepository _prescriptionRepository;
        private List<Prescription> _prescriptions;


        public PrescriptionService()
        {
            this._prescriptionRepository = new PrescriptionRepository();
            this._prescriptions = _prescriptionRepository.Load();
        }

        public PrescriptionRepository PrescriptionRepository { get { return _prescriptionRepository; } set { _prescriptionRepository = value; } }
        public List<Prescription> Prescriptions { get { return _prescriptions; } set { _prescriptions = value; } }

    }
}
