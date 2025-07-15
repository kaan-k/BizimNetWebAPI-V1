using Autofac;
using Business.Abstract;
using Business.Concrete;
using Business.Concrete.Constants;
using DataAccess.Abstract;
using DataAccess.Concrete;
using DataAccess.Concrete.DataBases.MongoDB;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            
            builder.RegisterType<CustomerManager>().As<ICustomerService>().InstancePerLifetimeScope();
            builder.RegisterType<Mongo_CustomerDal>().As<ICustomerDal>().InstancePerLifetimeScope();

            builder.RegisterType<BusinessUserManager>().As<IBusinessUserService>().InstancePerLifetimeScope();
            builder.RegisterType<Mongo_BusinessUserDal>().As<IBusinessUserDal>().InstancePerLifetimeScope();

            builder.RegisterType<DepartmentManager>().As<IDepartmentService>().InstancePerLifetimeScope();
            builder.RegisterType<Mongo_DepartmentDal>().As<IDepartmentDal>().InstancePerLifetimeScope();

            builder.RegisterType<EmployeeManager>().As<IEmployeeService>().InstancePerLifetimeScope();
            builder.RegisterType<Mongo_EmployeeDal>().As<IEmployeeDal>().InstancePerLifetimeScope();

            builder.RegisterType<DocumentFileManager>().As<IDocumentFileUploadService>().InstancePerLifetimeScope();
            builder.RegisterType<MongoDB_DocumentFileUpload>().As<IDocumentFileUploadDal>().InstancePerLifetimeScope();

            builder.RegisterType<OfferManager>().As<IOfferService>().InstancePerLifetimeScope();
            builder.RegisterType<Mongo_OfferDal>().As<IOfferDal>().InstancePerLifetimeScope();

            builder.RegisterType<InstallationRequestManager>().As<IInstallationRequestService>().InstancePerLifetimeScope();
            builder.RegisterType<Mongo_InstallationRequestDal>().As<IInstallationRequestDal>().InstancePerLifetimeScope();

            builder.RegisterType<DeviceManager>().As<IDeviceService>().InstancePerLifetimeScope();
            builder.RegisterType<Mongo_DeviceDal>().As<IDeviceDal>().InstancePerLifetimeScope();

            builder.RegisterType<ServicingManager>().As<IServicingService>().InstancePerLifetimeScope();
            builder.RegisterType<Mongo_ServicingDal>().As<IServicingDal>().InstancePerLifetimeScope();

            builder.RegisterType<StockManager>().As<IStockService>().InstancePerLifetimeScope();
            builder.RegisterType<Mongo_StockDal>().As<IStockDal>().InstancePerLifetimeScope();

            builder.RegisterType<MailManager>().As<IMailManager>().InstancePerLifetimeScope();

            builder.RegisterType<PdfGeneratorManager>().As<IPdfGeneratorService>().SingleInstance();

        }
    }
}
