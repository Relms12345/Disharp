using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Disharp.Constants
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum TokenType
	{
		Bot,
		Bearer
	}
}