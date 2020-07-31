using System;
using System.Collections.Generic;
using Disharp.Constants;

namespace Disharp.Utils
{
	public class IntentsBuilder
	{
		public IntentsBuilder(IEnumerable<Intents> intents)
		{
			foreach (var intent in intents) Intent += (int) Convert.ChangeType(intent, intent.GetTypeCode());
		}

		public int Intent { get; set; }
	}
}