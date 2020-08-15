using System;

namespace MPL
{
	class FuncWrapper<T> : Runnable where T : Message
	{
		private ProcessId to;
		private Func<T> func;

		public FuncWrapper(ProcessId pid, Func<T> f)
		{
			to = pid;
			func = f;
		}

		public override void Invoke(MessagingRuntime rt, Mailbox mbox)
		{
			rt.Send(to, func());
		}
	}
}