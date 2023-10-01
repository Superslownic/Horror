using System.Collections.Generic;

namespace Scripts.Entities
{
	public abstract class ActivatableAbility : Ability
	{
		public HashSet<object> Activators { get; } = new();
		
		public void AddActivator(object activator)
		{
			if (Activators.Add(activator))
			{
				SetActive(Activators.Count > 0);
			}
		}

		public void RemoveActivator(object activator)
		{
			if (Activators.Remove(activator))
			{
				SetActive(Activators.Count > 0);
			}
		}
	}
}