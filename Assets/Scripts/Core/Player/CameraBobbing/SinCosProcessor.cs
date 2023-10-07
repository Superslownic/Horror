using UnityEngine;

namespace Scripts.Core.Player
{
	public abstract class SinCosProcessor : ICameraBobbingProcessor
	{
		[SerializeField] protected float _frequency;
		[SerializeField] protected float _amplitude;
		
		protected float _timer;
		
		public void Execute(Transform transform, float strength)
		{
			_timer += Time.deltaTime * strength;
			ExecuteInternal(transform, strength);
		}

		protected abstract void ExecuteInternal(Transform transform, float strength);
	}
}