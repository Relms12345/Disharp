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
			manager = new RestManager(Client, this);
			cdn = new CDN(Client.ClientOptions.RestOptions.CdnUrl.ToString());
		}

		public CDN cdn { get; set; }

		private RestManager manager { get; }
		private DisharpClient Client { get; }

		public Task<dynamic> Get(string endpoint, RestRequestOptions options)
		{
			return manager.QueueRequest(new RestReq
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
			return manager.QueueRequest(new RestReq
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
			return manager.QueueRequest(new RestReq
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
			return manager.QueueRequest(new RestReq
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
			return manager.QueueRequest(new RestReq
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