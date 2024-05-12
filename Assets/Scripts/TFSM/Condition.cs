using System;

namespace Scripts.TFSM
{
	public class Condition
	{
		public bool IsValid => _predicate.Invoke();

		private Func<bool> _predicate;

		public Condition(Func<bool> predicate)
		{
			_predicate = predicate;
		}

		public static implicit operator bool(Condition condition) => condition.IsValid;
	}
}