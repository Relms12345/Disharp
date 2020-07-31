using RestSharp;

namespace Disharp.Types
{
	public class RestReq : RestRequestOptions
	{
		public Method Method { get; set; }
		public string Endpoint { get; set; }
	}
}