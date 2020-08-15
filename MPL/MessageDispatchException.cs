using System;

namespace MPL
{
	public class MessageDispatchException : Exception
	{
		private MessageDispatchException(string msg) : base(msg) {}

		public static MessageDispatchException Format(Type runnable, Type msg)
		{
			return new MessageDispatchException($"{runnable} has no Receive({msg}) method");
		}
	}
}