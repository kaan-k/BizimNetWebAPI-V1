using Autofac;
using Business.Abstract;
using Business.Concrete;
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
        }
    }
}
