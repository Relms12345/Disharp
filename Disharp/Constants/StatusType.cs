using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Disharp.Constants
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum StatusType
	{
		[EnumMember(Value = "online")]
		Online,
		
		[EnumMember(Value = "dnd")]
		Dnd,
		
		[EnumMember(Value = "idle")]
		Idle,
		
		[EnumMember(Value = "invisible")]
		Invisible,
		
		[EnumMember(Value = "offline")]
		Offline
	}
}