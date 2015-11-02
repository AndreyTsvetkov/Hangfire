using System.ComponentModel;
using System.Threading;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Microsoft.Owin;
using MvcSample;
using Owin;
using System;

[assembly: OwinStartup(typeof(Startup))]

namespace MvcSample
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			GlobalConfiguration.Configuration
				.UseSqlServerStorage(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|Schedule.mdf;Database=HangfireSample;Integrated Security=True")
				.UseDashboardMetric(SqlServerStorage.ActiveConnections)
				.UseDashboardMetric(SqlServerStorage.TotalConnections)
				.UseDashboardMetric(DashboardMetrics.FailedCount);

			GlobalConfiguration.Configuration.UseNLogJobLogs();

			app.UseHangfireDashboard("");
			app.UseHangfireServer();

			RecurringJob.AddOrUpdate("Failing task", () => WorkAndLog("Failing task"), "* * * * *");
		}

		[DisplayName("WorkAndLog: {0}")]
		public static void WorkAndLog(string hangfireDisplayName)
		{
			NLog.LogManager.GetCurrentClassLogger().Info("Started");
			
			Thread.Sleep(TimeSpan.FromSeconds(5));
	
			NLog.LogManager.GetCurrentClassLogger().Info("Failing");

			throw new Exception("I am bad");
		}
	}
}
