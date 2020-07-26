using System;
using System.Collections.Generic;

namespace Disharp.Constants
{
	public static class DefaultClientOptions
	{
		private static Dictionary<string, dynamic> _defaultOptions { get; set; }

		public static void SetDefaultOptions()
		{
			var Dict = new Dictionary<string, dynamic>();
			var WsOptions = new Dictionary<string, dynamic>();
			
			WsOptions.Add("Compress", true);
			WsOptions.Add("EncodingType", EncodingType.Json.ToString());
			WsOptions.Add("BaseGatewayUrl", new Uri("wss://gateway.discord.gg/"));
			
			Dict.Add("WsOptions", WsOptions);

			_defaultOptions = Dict;
		}
		
		public static Dictionary<string, dynamic> GetDefaultOptions()
		{
			return _defaultOptions;
		}
	}
}