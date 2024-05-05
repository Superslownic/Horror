using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Scripts.Behaviour
{
	public class DeactivationController : IDeactivationController
	{
		[ShowInInspector] public HashSet<object> Deactivators { get; } = new();

		public bool Add(object deactivator)
		{
			return Deactivators.Add(deactivator);
		}

		public bool Remove(object deactivator)
		{
			return Deactivators.Remove(deactivator);
		}

		public bool GetState()
		{
			return Deactivators.Count == 0;
		}
	}
}