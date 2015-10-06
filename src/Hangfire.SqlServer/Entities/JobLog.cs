using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.SqlServer.Entities
{
	internal class JobLog
	{
		public int JobId { get; set; }
		public DateTime CreatedAt { get; set; }
		public string MessageClass { get; set; }
		public string MessageText { get; set; }
	}
}
