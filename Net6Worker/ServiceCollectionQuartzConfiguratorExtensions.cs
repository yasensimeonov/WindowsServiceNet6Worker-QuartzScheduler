using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net6Worker
{
    public static class ServiceCollectionQuartzConfiguratorExtensions
    {
        public static void AddJobAndTrigger<T>(this IServiceCollectionQuartzConfigurator quartz, IConfiguration config) where T : IJob
        {
            // Use the name of the IJob as the appsettings.json key
            string jobName = typeof(T).Name;

            // Try and load the schedule from configuration
            var configKey = $"Quartz:{jobName}";
            //var cronSchedule = config[configKey];
            var intervalInMinutes = config[configKey];

            // Some minor validation
            if (string.IsNullOrEmpty(intervalInMinutes))
            {
                throw new Exception($"No Quartz.NET Interval in Minutes schedule found for job in configuration at {configKey}");
            }

            // register the job as before
            var jobKey = new JobKey(jobName);
            quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));
            
            quartz.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(jobName + "-trigger")
                .WithSimpleSchedule(sb => sb
                    .WithIntervalInMinutes(Convert.ToInt32(intervalInMinutes))
                    .RepeatForever()
                    )
                );
                //.WithCronSchedule(cronSchedule)); // use the schedule from configuration
        }
    }
}
