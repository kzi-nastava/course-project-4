using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hospital.Service;
using Hospital.Model;

namespace Hospital.DoctorImplementation
{
    class DynamicEquipmentRecords
    {
        DynamicRoomEquipmentService dynamicEquipmentService;
        List<DynamicRoomEquipment> dynamicEquipments;
        WarehouseService warehouseService;


        public DynamicEquipmentRecords()
        {
            dynamicEquipmentService = new DynamicRoomEquipmentService();
            dynamicEquipments = dynamicEquipmentService.DynamicEquipments;
            warehouseService = new WarehouseService();
        }

        public void  DisplayAmountOfDynamicEquipments(string idRoom)
        {
            Console.WriteLine("------------------Stanje dinamičke opreme-----------------------");
            Console.WriteLine(String.Format("|{0,5}|{1,10}|{2,10}", "Br.", "Oprema", "Količina"));
            int serialNumber = 1;
            foreach(DynamicRoomEquipment equipment in dynamicEquipments)
            {
                if (equipment.IdRoom.Equals(idRoom))
                {
                    foreach (KeyValuePair<string, int> pair in equipment.AmountEquipment)
                    {
                        Console.WriteLine("|{0,5}|{1,10}|{2,10}|",serialNumber, warehouseService.GetNameEquipment(pair.Key), pair.Value);
                        serialNumber += 1;
                    }
                    this.EntryOfSpentEquipment(equipment, idRoom);
                }
               
            }
        }

        private void EntryOfSpentEquipment(DynamicRoomEquipment equipment, string idRoom)
        {
            string amount;
            int temp;
            Dictionary<string, int> remainingAmountAfterAppointment = new Dictionary<string, int>();
            foreach(KeyValuePair<string, int> pair in equipment.AmountEquipment)
            {
                do
                {
                    do
                    {
                        Console.WriteLine("Unesite koliko ste potrosili " + warehouseService.GetNameEquipment(pair.Key)  + ": ");
                        amount = Console.ReadLine();
                    } while (!int.TryParse(amount, out temp));

                } while (int.Parse(amount) > pair.Value);

                remainingAmountAfterAppointment.Add(pair.Key, pair.Value - int.Parse(amount));
            }
            dynamicEquipmentService.UpdateDictionary(remainingAmountAfterAppointment, idRoom);
            dynamicEquipmentService.UpdateFile();
            Console.WriteLine("Uspesno ste uneli svu potrosenu robu!");

        }
    }
}
