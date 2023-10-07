using UnityEngine;

namespace Scripts.Core.Camera
{
	public interface ICameraBobbingProcessor
	{
		void Execute(Transform transform, float strength);
	}
}