using System.Collections.Concurrent;
using System.Threading;

namespace MPL
{
	public class Mailbox
	{
		private ConcurrentQueue<Message> Messages { get; }
		private object receiveCondition = new object();
		private Runnable Unit { get; }

		internal Mailbox(Runnable unit)
		{
			Unit = unit;
			Messages = new ConcurrentQueue<Message>();
		}

		internal void Deliver<T>(T msg) where T : Message
		{
			Messages.Enqueue(msg);
			lock (receiveCondition)
			{
				Monitor.Pulse(receiveCondition);
			}
		}

		public void Receive()
		{
			while (true)
			{
				if (Messages.TryDequeue(out Message msg))
				{
					Unit.Dispatch(msg);
					return;
				}

				lock(receiveCondition)
				{
					Monitor.Wait(receiveCondition);
				}
			}
		}
	}
}