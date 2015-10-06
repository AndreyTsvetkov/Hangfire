using System;

namespace Hangfire.Storage.Monitoring
{
	public class JobLogDto
	{
		public DateTime CreatedAt { get; set; }

		public string MessageClass { get; set; }

		public string MessageText { get; set; }
	}
}