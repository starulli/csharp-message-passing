using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MPL
{
	public class MessagingRuntime
	{
		private ConcurrentDictionary<ProcessId, Process> processSpace =
			new ConcurrentDictionary<ProcessId, Process>();
		private object processCondition = new object();

		public ProcessId Spawn(Runnable unit)
		{
			var process = new Process(unit);
			ProcessId id;
			var added = false;
			do
			{
				id = new ProcessId();
				process.Id = id;
				unit.Self = id;
				added = processSpace.TryAdd(id, process);
			}
			while (!added);

			Task.Run(() => process.Run(this)).ContinueWith((Task _t) => {
				var removed = false;
				do
				{
					removed = processSpace.TryRemove(id, out _);
				}
				while (!removed);

				lock(processCondition)
				{
					Monitor.Pulse(processCondition);
				}
			});

			return id;
		}

		public ProcessId Spawn<T>(ProcessId pid, Func<T> f) where T : Message
		{
			return Spawn(new FuncWrapper<T>(pid, f));
		}

		public T Send<T>(ProcessId to, T msg) where T : Message
		{
			var found = processSpace.TryGetValue(to, out Process p);
			if (found)
			{
				p.Receive(msg);
			}

			return msg;
		}

		public void Wait()
		{
			while (processSpace.Count > 0)
			{
				lock(processCondition)
				{
					Monitor.Wait(processCondition);
				}
			}
		}
	}
}