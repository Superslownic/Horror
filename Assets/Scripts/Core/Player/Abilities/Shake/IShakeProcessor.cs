using UnityEngine;

namespace Scripts.Core.Player.Shake
{
	public interface IShakeProcessor
	{
		void Reset();
		void Update(out Vector3 position, out Vector3 rotation);
	}
}