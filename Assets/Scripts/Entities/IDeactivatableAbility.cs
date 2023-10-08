using UnityEngine;

namespace Scripts.Entities
{
	public interface IDeactivatableAbility
	{
		GameObject gameObject { get; }
		void AddDeactivator(object deactivator);
		void RemoveDeactivator(object deactivator);
	}
}