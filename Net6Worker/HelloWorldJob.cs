using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Net6Worker
{
    [DisallowConcurrentExecution]
    public class HelloWorldJob : IJob
    {
        private readonly ILogger<HelloWorldJob> _logger;

        public HelloWorldJob(ILogger<HelloWorldJob> logger)
        {
            _logger = logger;
        }

        //public Task Execute(IJobExecutionContext context)
        //{
        //    _logger.LogInformation("Hello world!");
        //    return Task.CompletedTask;
        //}

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {                
                string path = @"C:\\Temp\\Net6WorkerLogs.txt";
                await using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine("Log Time: " + DateTime.Now);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Exception: " + ex.Message);
            }
            
            await Task.CompletedTask;
        }

    }

}
