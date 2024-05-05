using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Scripts.Behaviour
{
	public class ActivationController : IActivationController
	{
		[ShowInInspector] public HashSet<object> Activators { get; } = new();

		public bool Add(object activator)
		{
			return Activators.Add(activator);
		}

		public bool Remove(object activator)
		{
			return Activators.Remove(activator);
		}

		public bool GetState()
		{
			return Activators.Count > 0;
		}
	}
}