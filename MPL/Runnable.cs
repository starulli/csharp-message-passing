using System;

namespace MPL
{
	public abstract class Runnable
	{
		public abstract void Invoke(MessagingRuntime rt, Mailbox mbox);

		public ProcessId Self { get; internal set; }

		public void Dispatch(Message msg)
		{
			var runnableType = GetType();
			var msgType = msg.GetType();
			Type[] types = {msgType};
			
			var method = runnableType.GetMethod("Receive", types);
			if (method == null)
			{
				throw MessageDispatchException.Format(runnableType, msgType);
			}

			object[] args = {msg};
			method.Invoke(this, args);
		}
	}
}