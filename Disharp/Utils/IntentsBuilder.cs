using System;
using System.Linq;
using Disharp.Constants;

namespace Disharp.Utils
{
	public class IntentsBuilder
	{
		public int Intent { get; set; } = 0;
		
		public IntentsBuilder(Intents[] intents)
		{
			foreach (var intent in intents)
			{
				Intent += (int)Convert.ChangeType(intent, intent.GetTypeCode());
			}
		}
	}
}