using System.Threading;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Microsoft.Owin;
using MvcSample;
using NLog.Targets;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace MvcSample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration
                .UseSqlServerStorage(@"Server=.;Database=Hangfire.Sample;Trusted_Connection=True;")
                .UseDashboardMetric(SqlServerStorage.ActiveConnections)
                .UseDashboardMetric(SqlServerStorage.TotalConnections)
                .UseDashboardMetric(DashboardMetrics.FailedCount);

	        GlobalConfiguration.Configuration.UseNLogJobLogs();

			app.UseHangfireDashboard("");
			app.UseHangfireServer();

			RecurringJob.AddOrUpdate("test 3", () => WorkAndLog(), "* * * * *");
		}

	    public static void WorkAndLog()
	    {
			NLog.LogManager.GetCurrentClassLogger().Debug("Debug");
			NLog.LogManager.GetCurrentClassLogger().Trace("Trace");
			NLog.LogManager.GetCurrentClassLogger().Info("Info");
			
			Thread.Sleep(1000);
			
			NLog.LogManager.GetCurrentClassLogger().Warn("Warn");
			NLog.LogManager.GetCurrentClassLogger().Error("Error");
	//		NLog.LogManager.GetCurrentClassLogger().Fatal("Fatal");
		}
    }
}
