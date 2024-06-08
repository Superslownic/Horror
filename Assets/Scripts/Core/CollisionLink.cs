using Scripts.Reactive;
using UnityEngine;

namespace Scripts.Core
{
	public class CollisionLink : MonoBehaviour
	{
		public DisposableAction<Collision> OnEnter { get; } = new();
		public DisposableAction<Collision> OnStay { get; } = new();
		public DisposableAction<Collision> OnExit { get; } = new();

		private void OnCollisionEnter(Collision collision)
		{
			OnEnter.Invoke(collision);
		}

		private void OnCollisionStay(Collision collision)
		{
			OnStay.Invoke(collision);
		}

		private void OnCollisionExit(Collision collision)
		{
			OnExit.Invoke(collision);
		}
	}
}