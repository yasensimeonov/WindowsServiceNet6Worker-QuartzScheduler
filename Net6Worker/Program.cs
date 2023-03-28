using Net6Worker;
using Quartz;
using System.Configuration;
using System.Runtime.CompilerServices;

//IHost host = Host.CreateDefaultBuilder(args)
//    .ConfigureServices(services =>
//    {
//        //services.AddHostedService<Worker>();
//        ConfigureQuartzService(services);
//    })
//    .Build();

var host = CreateHostBuilder(args).Build();
await host.RunAsync();

IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            ConfigureQuartzService(services, hostContext);
        });

void ConfigureQuartzService(IServiceCollection services, HostBuilderContext hostContext)
{
    // Add the required Quartz.NET services
    services.AddQuartz(q =>
    {
        // Use a Scoped container to create jobs.
        q.UseMicrosoftDependencyInjectionJobFactory();

        //// Create a "key" for the job
        //var jobKey = new JobKey("HelloWorldJob");

        //// Register the job with the DI container
        //q.AddJob<HelloWorldJob>(opts => opts.WithIdentity(jobKey));

        //// Create a trigger for the job
        //q.AddTrigger(opts => opts
        //    .ForJob(jobKey) // link to the HelloWorldJob
        //    .WithIdentity("HelloWorldJob-trigger") // give the trigger a unique name
        //    .WithCronSchedule("0/5 * * * * ?")); // run every 5 seconds

        // Register the job, loading the schedule from configuration
        q.AddJobAndTrigger<HelloWorldJob>(hostContext.Configuration);
    });

    // Add the Quartz.NET hosted service
    services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

    // Other config
}