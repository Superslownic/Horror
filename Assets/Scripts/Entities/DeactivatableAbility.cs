using System.Collections.Generic;

namespace Scripts.Entities
{
	public abstract class DeactivatableAbility : Ability
	{
		public HashSet<object> Deactivators { get; } = new();
		
		public void AddDeactivator(object deactivator)
		{
			if (Deactivators.Add(deactivator))
			{
				SetActive(Deactivators.Count == 0);
			}
		}

		public void RemoveDeactivator(object deactivator)
		{
			if (Deactivators.Remove(deactivator))
			{
				SetActive(Deactivators.Count == 0);
			}
		}
	}
}