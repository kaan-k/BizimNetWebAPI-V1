    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using BizimNetWorker;
    using BizimNetWorker.Jobs;
    using Business.DependencyResolvers.Autofac;
    using Core.DependencyResolvers;
    using Core.Extensions;
    using Core.Utilities.IoC;
    using DataAccess.DependencyResolvers;
    using Entities.Profiles.AutoMapperProfiles;
    using Microsoft.Extensions.DependencyInjection;
    using Quartz;
    using Quartz.Impl;
    using Quartz.Spi;

var host = Host.CreateDefaultBuilder(args)
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterModule(new AutofacBusinessModule());
        builder.RegisterModule(new AutoFacCoreModule());
        builder.RegisterModule(new AutoFacDataAccessModule());

    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddAutoMapper(typeof(EntitiesAutoMapperProfile));
        services.AddQuartz(q =>
        {
            var installationEscalationJobKey = new JobKey("InstallationRequestEscalationJob");

            q.AddJob<InstallationRequestEscalationJob>(installationEscalationJobKey, j => j
                .WithDescription("Installation Request Escalation Job")
            );

            q.AddTrigger(t => t
                .WithIdentity("InstallationRequestEscalationJobTrigger")
                .ForJob(installationEscalationJobKey)
                .StartNow()
                .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(1)
                .RepeatForever()
                )
            );
            var offerEscalationJobKey = new JobKey("OfferEscalationJob");

            q.AddJob<OfferEscalationJob>(offerEscalationJobKey, j => j
                .WithDescription("Offer Escalation Job")
            );

            q.AddTrigger(t => t
                .WithIdentity("OfferEscalationJobTrigger")
                .ForJob(offerEscalationJobKey)
                .StartNow()
                .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(1)
                .RepeatForever()
                )
            );
        });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

    })

    .Build();

host.Run();