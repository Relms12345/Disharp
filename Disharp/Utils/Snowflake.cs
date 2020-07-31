using System;

namespace Disharp.Utils
{
	public static class Snowflake
	{
		public static DateTimeOffset FromSnowflake(string value)
		{
			return DateTimeOffset.FromUnixTimeMilliseconds((Convert.ToInt64(value) >> 22) + 1420070400000L);
		}

		public static string ToSnowflake(DateTimeOffset value)
		{
			return Convert.ToString((value.ToUnixTimeMilliseconds() - 1420070400000L) << 22);
		}
	}
}