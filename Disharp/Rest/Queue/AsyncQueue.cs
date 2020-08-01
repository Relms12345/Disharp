using System.Collections.Generic;
using System.Threading.Tasks;
using Disharp.Utils;

namespace Disharp.Rest.Queue
{
	public class AsyncQueue
	{
		private List<InternalAsyncQueueDeferredTask> Tasks { get; } = new List<InternalAsyncQueueDeferredTask>();

		public int Remaining()
		{
			return Tasks.Count;
		}

		public void Wait()
		{
			var promise = new Task<InternalAsyncQueueDeferredTask>(() => new InternalAsyncQueueDeferredTask());

			Tasks.Add(new InternalAsyncQueueDeferredTask
			{
				Promise = promise
			});
		}

		public async void Shift()
		{
			var deferred = Tasks.ToArray()[0];
			ShiftArray.ShiftLeft(Tasks.ToArray(), 1);
			await Task.Run(() => deferred.Promise);
		}
	}

	public class InternalAsyncQueueDeferredTask
	{
		public Task<InternalAsyncQueueDeferredTask> Promise { get; set; }
	}
}