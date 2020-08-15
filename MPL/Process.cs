using System;

namespace MPL
{
	class Process
	{
		public ProcessId Id { get; internal set; }
		private Runnable Unit { get; }
		private Mailbox Mailbox { get; }

		public Process(Runnable unit)
		{
			Unit = unit;
			Mailbox = new Mailbox(unit);
		}

		public void Run(MessagingRuntime rt)
		{
			try
			{
				Unit.Invoke(rt, Mailbox);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(e.Message);
			}
		}

		public void Receive<T>(T msg) where T : Message
		{
			Mailbox.Deliver(msg);
		}

		public override string ToString()
		{
			return $"Process<Id={Id}>";
		}
	}
}