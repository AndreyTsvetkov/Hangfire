using System.ComponentModel;
using System.Threading;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Microsoft.Owin;
using MvcSample;
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

			RecurringJob.AddOrUpdate("test 4", () => WorkAndLog("Task 3"), "* * * * *");
		}

		[DisplayName("WorkAndLog: {0}")]
	    public static void WorkAndLog(string hangfireDisplayName)
	    {
			NLog.LogManager.GetCurrentClassLogger().Info("Started");
			Thread.Sleep(1000 * 60 * 5);
			NLog.LogManager.GetCurrentClassLogger().Info("Done");
		}
    }
}
