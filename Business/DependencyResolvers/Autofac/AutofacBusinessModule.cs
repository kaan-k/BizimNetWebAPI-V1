using Autofac;
using Business.Abstract;
using Business.Concrete;
using Core.Infrastructure;
using Core.Utilities.Context;
using DataAccess.Abstract;
using DataAccess.Concrete;

using DataAccess.Concrete.EntityFramework;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // --- Customer ---
            builder.RegisterType<CustomerManager>().As<ICustomerService>().InstancePerLifetimeScope();
            builder.RegisterType<EfCustomerDal>().As<ICustomerDal>().InstancePerLifetimeScope();

            // --- Agreement (Fixed Spelling: Aggrement -> Agreement) ---
            builder.RegisterType<AgreementManager>().As<IAggrementService>().InstancePerLifetimeScope();
            builder.RegisterType<EfAggrementDal>().As<IAggrementDal>().InstancePerLifetimeScope();

            // --- Billing ---
            builder.RegisterType<BillingManager>().As<IBillingService>().InstancePerLifetimeScope();
            builder.RegisterType<EfBillingDal>().As<IBillingDal>().InstancePerLifetimeScope();

            // --- Business User (Switched Mongo -> EF) ---
            builder.RegisterType<BusinessUserManager>().As<IBusinessUserService>().InstancePerLifetimeScope();
            builder.RegisterType<EfBusinessUserDal>().As<IBusinessUserDal>().InstancePerLifetimeScope();

            // --- Department ---
            builder.RegisterType<DepartmentManager>().As<IDepartmentService>().InstancePerLifetimeScope();
            builder.RegisterType<EfDepartmentDal>().As<IDepartmentDal>().InstancePerLifetimeScope();

            // --- Employee ---
            builder.RegisterType<EmployeeManager>().As<IEmployeeService>().InstancePerLifetimeScope();
            builder.RegisterType<EfEmployeeDal>().As<IEmployeeDal>().InstancePerLifetimeScope();

            // --- Document File (Standardized Name: EfDocumentFileUpload -> EfDocumentFileUploadDal) ---
            builder.RegisterType<DocumentFileManager>().As<IDocumentFileUploadService>().InstancePerLifetimeScope();
            builder.RegisterType<EfDocumentFileUploadDal>().As<IDocumentFileUploadDal>().InstancePerLifetimeScope();

            // --- Offer ---
            builder.RegisterType<OfferManager>().As<IOfferService>().InstancePerLifetimeScope();
            builder.RegisterType<EfOfferDal>().As<IOfferDal>().InstancePerLifetimeScope();

            // --- Installation Request ---
            builder.RegisterType<InstallationRequestManager>().As<IInstallationRequestService>().InstancePerLifetimeScope();
            builder.RegisterType<EfInstallationRequestDal>().As<IInstallationRequestDal>().InstancePerLifetimeScope();

            // --- Device ---
            builder.RegisterType<DeviceManager>().As<IDeviceService>().InstancePerLifetimeScope();
            builder.RegisterType<EfDeviceDal>().As<IDeviceDal>().InstancePerLifetimeScope();

            // --- Servicing (Switched Mongo -> EF) ---
            builder.RegisterType<ServicingManager>().As<IServicingService>().InstancePerLifetimeScope();
            builder.RegisterType<EfServicingDal>().As<IServicingDal>().InstancePerLifetimeScope();

            // --- Stock ---
            builder.RegisterType<StockManager>().As<IStockService>().InstancePerLifetimeScope();
            builder.RegisterType<EfStockDal>().As<IStockDal>().InstancePerLifetimeScope();

            // --- Duty ---
            builder.RegisterType<DutyManager>().As<IDutyService>().InstancePerLifetimeScope();
            builder.RegisterType<EfDutyDal>().As<IDutyDal>().InstancePerLifetimeScope();

            // --- AgGrid Settings ---
            builder.RegisterType<AgGridSettingsManager>().As<IAgGridSettingsService>().InstancePerLifetimeScope();
            builder.RegisterType<EfAgGridSettingsDal>().As<IAgGridSettingsDal>().InstancePerLifetimeScope();

            // --- Utilities ---
            builder.RegisterType<HttpUserContext>().As<IUserContext>().InstancePerLifetimeScope();
            builder.RegisterType<MailManager>().As<IMailManager>().InstancePerLifetimeScope();
            builder.RegisterType<PdfGeneratorManager>().As<IPdfGeneratorService>().SingleInstance();
        }
    }
}