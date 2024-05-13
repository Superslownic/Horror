using Scripts.Reactive;
using Scripts.Units;
using UnityEngine;

namespace Scripts.Core
{
	public class TriggerLink : MonoBehaviour
	{
		public DisposableAction<Unit> OnEnter { get; } = new();
		public DisposableAction<Unit> OnStay { get; } = new();
		public DisposableAction<Unit> OnExit { get; } = new();

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Unit entity))
			{
				OnEnter.Invoke(entity);
			}
			else if (other.TryGetComponent(out UnitLink entityProvider))
			{
				OnEnter.Invoke(entityProvider.Unit);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out Unit entity))
			{
				OnExit.Invoke(entity);
			}
			else if (other.TryGetComponent(out UnitLink entityProvider))
			{
				OnExit.Invoke(entityProvider.Unit);
			}
		}
	}
}