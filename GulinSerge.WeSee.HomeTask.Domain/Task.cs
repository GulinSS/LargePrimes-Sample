namespace GulinSerge.WeSee.HomeTask.Domain
{
	/// <summary>
	/// Вычислительная задача
	/// </summary>
	public class Task
	{
		public Task(ulong @from, ulong to)
		{
			From = @from;
			To = to;
		}

		/// <summary>
		/// Левая граница
		/// </summary>
		public ulong From { get; private set; }
		
		/// <summary>
		/// Правая граница
		/// </summary>
		public ulong To { get; private set; }

		public bool Equals(Task other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return other.From == From && other.To == To;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (Task)) return false;
			return Equals((Task) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (From.GetHashCode()*397) ^ To.GetHashCode();
			}
		}
	}
}