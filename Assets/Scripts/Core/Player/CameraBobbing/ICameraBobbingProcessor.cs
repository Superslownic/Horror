using UnityEngine;

namespace Scripts.Core.Player
{
	public interface ICameraBobbingProcessor
	{
		void Execute(Transform transform, float strength);
	}
}