using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Entities
{
	public class Deactivator : MonoBehaviour
	{
		public HashSet<object> Deactivators { get; } = new();

		private Ability _ability;

		private void Awake()
		{
			_ability = GetComponent<Ability>();
		}

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

		private void UpdateActivation()
		{
			_ability.SetActive(Deactivators.Count == 0);
		}
	}
}