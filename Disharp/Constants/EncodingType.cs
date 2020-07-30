using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Disharp.Constants
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum EncodingType
	{
		[JsonProperty("json")] Json
	}
}