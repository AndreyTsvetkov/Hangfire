using System;
using Hangfire.Server;
using NLog.Targets;
using NLog;

namespace Hangfire
{
	public class JobLogContext : IServerFilter
	{
		[ThreadStatic]
		private static string _jobId;

		private static string JobId { get { return _jobId; } set { _jobId = value; } }

		public void OnPerforming(PerformingContext context)
		{
			JobId = context.BackgroundJob.Id;
		}

		public void OnPerformed(PerformedContext filterContext)
		{
			JobId = null;
		}

		// ReSharper disable once UnusedMember.Global - used by NLog
		public static void Log(string @class, string text)
		{
			if (JobId != null)
			{
				using (var connection = JobStorage.Current.GetConnection())
				using (var transaction = connection.CreateWriteTransaction())
				{
					transaction.AddJobLog(JobId, @class, text);
					transaction.Commit();
				}
			}
		}
	}

	public static class NLogToJobLogHelper
	{
		// ReSharper disable once UnusedMethodReturnValue.Global
		public static IGlobalConfiguration UseNLogJobLogs(this IGlobalConfiguration configuration)
		{
			configuration.UseFilter(new JobLogContext());

			MethodCallTarget target = new MethodCallTarget
			{
				ClassName = typeof (JobLogContext).AssemblyQualifiedName,
				MethodName = "Log",
				Parameters = { new MethodCallParameter("${level}"), new MethodCallParameter("${message}") }
			};

			NLog.Config.SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Debug);


			return configuration;
		}
	}
}