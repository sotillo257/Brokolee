using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using WebApplication1.Controllers;

namespace WebApplication1.ScheduledTasks
{
    //public class AlertManage : IJob
    //{
    //    async Task IJob.Execute(IJobExecutionContext context)
    //    {
           
    //        throw new NotImplementedException();
    //    }
    //}

    public class JobScheduler
    {
        public static void Start()
        {
            Task<IScheduler> scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Result.Start();

            //IJobDetail job = JobBuilder.Create<AlertManage>().Build();

            //ITrigger trigger = TriggerBuilder.Create()
            //    .WithDailyTimeIntervalSchedule
            //      (s =>
            //         s.WithIntervalInSeconds(5)
            //        .OnEveryDay()
            //        .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))
            //      )
            //    .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(3600)
                    .RepeatForever())
                .Build();

            //scheduler.Result.ScheduleJob(job, trigger);
        }
    }
}