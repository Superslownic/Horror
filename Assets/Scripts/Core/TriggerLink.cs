using Scripts.Reactive;
using Scripts.Units;
using Scripts.Utility.Extensions;
using UnityEngine;

namespace Scripts.Core
{
	public class TriggerLink : MonoBehaviour
	{
		public DisposableAction<Unit> OnEnter { get; } = new();
		public DisposableAction<Unit> OnExit { get; } = new();

		private void OnTriggerEnter(Collider other)
		{
			if(other.TryGetUnit(out Unit unit))
				OnEnter.Invoke(unit);
		}

		private void OnTriggerExit(Collider other)
		{
			if(other.TryGetUnit(out Unit unit))
				OnExit.Invoke(unit);
		}
	}
}