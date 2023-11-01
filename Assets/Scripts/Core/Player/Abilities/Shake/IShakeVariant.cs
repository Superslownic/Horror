using UnityEngine;

namespace Scripts.Core.Player
{
	public interface IShakeVariant
	{
		void Reset();
		void Update(out Vector3 position, out Vector3 rotation);
	}
}