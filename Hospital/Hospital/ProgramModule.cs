using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

using Hospital.Rooms.Repository;
using Hospital.Rooms.Service;
using Hospital.Users.Repository;
using Hospital.Users.Service;
using Hospital.Appointments.Repository;
using Hospital.Appointments.Service;
using Hospital.Drugs.Repository;
using Hospital.Drugs.Service;

namespace Hospital
{
    class ProgramModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppointmentRepository>().As<IAppointmentRepository>();
            builder.RegisterType<HealthRecordRepository>().As<IHealthRecordRepository>();
            builder.RegisterType<MedicalRecordRepository>().As<IMedicalRecordRepository>();
            builder.RegisterType<PrescriptionRepository>().As<IPrescriptionRepository>();
            //builder.RegisterType<PatientRequestRepository>().As<IPatientRequestRepository>();
            builder.RegisterType<ReferralRepository>().As<IReferralRepository>();

            builder.RegisterType<AppointmentService>().As<IAppointmentService>();
            builder.RegisterType<HealthRecordService>().As<IHealthRecordService>();
            builder.RegisterType<MedicalRecordService>().As<IMedicalRecordService>();
            //builder.RegisterType<PatientRequestService>().As<IPatientRequestService>();
            builder.RegisterType<PrescriptionService>().As<IPrescriptionService>();
            builder.RegisterType<ReferralService>().As<IReferralService>();

            builder.RegisterType<DrugNotificationRepository>().As<IDrugNotificationRepository>();
            builder.RegisterType<DrugProposalRepository>().As<IDrugProposalRepository>();
            builder.RegisterType<DrugRepository>().As<IDrugRepository>();
            builder.RegisterType<IngredientRepository>().As<IIngredientRepository>();

            builder.RegisterType<DrugNotificationService>().As<IDrugNotificationService>();
            builder.RegisterType<DrugProposalService>().As<IDrugProposalService>();
            builder.RegisterType<DrugService>().As<IDrugService>();
            builder.RegisterType<IngredientService>().As<IIngredientService>();

            //builder.RegisterType<DynamicEquipmentRequestRepository>().As<IDynamicEquipmentRequestRepository>();
            builder.RegisterType<DynamicRoomEquipmentRepository>().As<IDynamicRoomEquipmentRepository>();
            builder.RegisterType<EquipmentMovingRepository>().As<IEquipmentMovingRepository>();
            builder.RegisterType<EquipmentRepository>().As<IEquipmentRepository>();
            builder.RegisterType<RenovationRepository>().As<IRenovationRepository>();
            builder.RegisterType<RoomRepository>().As<IRoomRepository>();
            builder.RegisterType<WarehouseRepository>().As<IWarehouseRepository>();

            //builder.RegisterType<DynamicEquipmentRequestService>().As<IDynamicEquipmentRequestService>();
            builder.RegisterType<DynamicRoomEquipmentService>().As<IDynamicRoomEquipmentService>();
            builder.RegisterType<EquipmentMovingService>().As<IEquipmentMovingService>();
            builder.RegisterType<EquipmentService>().As<IEquipmentService>();
            builder.RegisterType<RenovationService>().As<IRenovationService>();
            builder.RegisterType<RoomService>().As<IRoomService>();
            builder.RegisterType<WarehouseService>().As<IWarehouseService>();

            builder.RegisterType<DoctorSurveyRepository>().As<IDoctorSurveyRepository>();
            builder.RegisterType<HospitalSurveyRepository>().As<IHospitalSurveyRepository>();
            //builder.RegisterType<NotificationRepository>().As<INotificationRepository>();
            builder.RegisterType<RequestForDaysOffRepository>().As<IRequestForDaysOffRepository>();
            builder.RegisterType<UserActionRepository>().As<IUserActionRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();

            builder.RegisterType<DoctorSurveyService>().As<IDoctorSurveyService>();
            builder.RegisterType<HospitalSurveyService>().As<IHospitalSurveyService>();
            //builder.RegisterType<NotificationService>().As<INotificationService>();
            builder.RegisterType<RequestForDaysOffService>().As<IRequestForDaysOffService>();
            builder.RegisterType<UserActionService>().As<IUserActionService>();
            builder.RegisterType<UserService>().As<IUserService>();
        }

    }
}

