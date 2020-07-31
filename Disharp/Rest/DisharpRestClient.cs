using System.Threading.Tasks;
using Disharp.Client;
using Disharp.Types;
using RestSharp;

namespace Disharp.Rest
{
	public class DisharpRestClient
	{
		public DisharpRestClient(DisharpClient client)
		{
			Client = client;
			Manager = new RestManager(Client, this);
			Cdn = new Cdn(Client.ClientOptions.RestOptions.CdnUrl.ToString());
		}

		public Cdn Cdn { get; set; }

		private RestManager Manager { get; }
		private DisharpClient Client { get; }

		public Task<dynamic> Get(string endpoint, RestRequestOptions options)
		{
			return Manager.QueueRequest(new RestReq
			{
				Auth = true,
				Data = options.Data,
				Endpoint = endpoint,
				Files = options.Files,
				Method = Method.GET,
				Reason = options.Reason,
				Query = options.Query
			});
		}

		public dynamic Delete(string endpoint, RestRequestOptions options)
		{
			return Manager.QueueRequest(new RestReq
			{
				Auth = true,
				Data = options.Data,
				Endpoint = endpoint,
				Files = options.Files,
				Method = Method.DELETE,
				Reason = options.Reason,
				Query = options.Query
			});
		}


		public dynamic Patch(string endpoint, RestRequestOptions options)
		{
			return Manager.QueueRequest(new RestReq
			{
				Auth = true,
				Data = options.Data,
				Endpoint = endpoint,
				Files = options.Files,
				Method = Method.PATCH,
				Reason = options.Reason,
				Query = options.Query
			});
		}


		public dynamic Put(string endpoint, RestRequestOptions options)
		{
			return Manager.QueueRequest(new RestReq
			{
				Auth = true,
				Data = options.Data,
				Endpoint = endpoint,
				Files = options.Files,
				Method = Method.PUT,
				Reason = options.Reason,
				Query = options.Query
			});
		}


		public dynamic Post(string endpoint, RestRequestOptions options)
		{
			return Manager.QueueRequest(new RestReq
			{
				Auth = true,
				Data = options.Data,
				Endpoint = endpoint,
				Files = options.Files,
				Method = Method.POST,
				Reason = options.Reason,
				Query = options.Query
			});
		}
	}
}