using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Entities
{
	public class Activator : MonoBehaviour
	{
		public HashSet<object> Activators { get; } = new();

		private Ability _ability;

		private void Awake()
		{
			_ability = GetComponent<Ability>();
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
		
		private void UpdateActivation()
		{
			_ability.SetActive(Activators.Count > 0);
		}
	}
}