using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disharp.Utils;
using RSG;

namespace Disharp.Rest.Queue
{
	public class AsyncQueue
	{
		private List<InternalAsyncQueueDeferredTask> Tasks { get; } = new List<InternalAsyncQueueDeferredTask>();

		public int Remaining()
		{
			return Tasks.Count;
		}

		public Task<InternalAsyncQueueDeferredTask> Wait()
		{
			var next = Tasks.Count != 0
				? Tasks.ToArray()[Tasks.Count - 1].Promise
				: new Task<InternalAsyncQueueDeferredTask>(() => new InternalAsyncQueueDeferredTask());
			Action<InternalAsyncQueueDeferredTask> resolve = null;

			var promise = new Task<InternalAsyncQueueDeferredTask>(() => new InternalAsyncQueueDeferredTask());

			Tasks.Add(new InternalAsyncQueueDeferredTask
			{
				Promise = promise
			});

			return next;
		}

		public void Shift()
		{
			var deferred = Tasks.ToArray()[0];
			ShiftArray.ShiftLeft(Tasks.ToArray(), 1);
			deferred.Promise.ConfigureAwait(true).GetAwaiter();
		}
	}

	public class InternalAsyncQueueDeferredTask
	{
		public Task<InternalAsyncQueueDeferredTask> Promise { get; set; }
	}
}