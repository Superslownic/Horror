using UnityEngine;

namespace Scripts.Entities
{
	public interface IActivatableAbility
	{
		GameObject gameObject { get; }
		void AddActivator(object activator);
		void RemoveActivator(object activator);
	}
}