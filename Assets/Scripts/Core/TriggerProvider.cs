using Scripts.Entities;
using Scripts.Reactive;
using UnityEngine;

namespace Scripts.Core
{
	public class TriggerProvider : MonoBehaviour
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
			
			if (other.TryGetComponent(out UnitProvider entityProvider))
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
			
			if (other.TryGetComponent(out UnitProvider entityProvider))
			{
				OnExit.Invoke(entityProvider.Unit);
			}
		}
	}
}