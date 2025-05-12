using Autofac;
using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete;

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


        }
    }
}
