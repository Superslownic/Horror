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
				UpdateActivation();
			}
		}

		public void RemoveDeactivator(object deactivator)
		{
			if (Deactivators.Remove(deactivator))
			{
				UpdateActivation();
			}
		}

		protected override void UpdateActivation()
		{
			SetActive(Deactivators.Count == 0);
		}
	}
}