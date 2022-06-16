using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Drugs.Model;
using Hospital.Drugs.Repository;

namespace Hospital.Drugs.Service
{
    public interface IDrugNotificationService: IService<DrugNotification>
    {
        DrugNotificationRepository DrugNotificationRepository { get; }
        List<DrugNotification> Notifications { get; set; }
        void ChangeNotificationTime(string patientEmail);

    }
}
