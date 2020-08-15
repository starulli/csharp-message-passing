using System;

namespace MPL
{
	public class ProcessId
	{
		private Guid id = Guid.NewGuid();

		public override int GetHashCode()
		{
			return id.GetHashCode();
		}

		public bool Equals(ProcessId other)
		{
			return id == other.id;
		}

		public override string ToString()
		{
			return id.ToString();
		}
	}
}