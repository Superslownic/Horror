using System.Collections.Generic;

namespace Scripts.Entities
{
	public abstract class ActivatableAbility : Ability
	{
		public HashSet<object> Activators { get; } = new();

		protected override void OnInitialize()
		{
			UpdateActivation();
		}

		public void AddActivator(object activator)
		{
			if (Activators.Add(activator))
			{
				UpdateActivation();
			}
		}

		public void RemoveActivator(object activator)
		{
			if (Activators.Remove(activator))
			{
				UpdateActivation();
			}
		}
		
		protected override void UpdateActivation()
		{
			SetActive(Activators.Count > 0);
		}
	}
}