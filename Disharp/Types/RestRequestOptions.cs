using System.Collections.Generic;
using RestSharp;

namespace Disharp.Types
{
	public class RestRequestOptions
	{
		public Dictionary<string, dynamic> Query { get; set; }
		public Dictionary<string, dynamic> Headers { get; set; }
		public dynamic Data { get; set; }
		public FileParameter[] Files { get; set; }
		public string Reason { get; set; }
		public bool Auth { get; set; }
	}
}