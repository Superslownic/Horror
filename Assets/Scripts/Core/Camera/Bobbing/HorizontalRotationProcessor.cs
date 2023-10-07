using System;
using UnityEngine;

namespace Scripts.Core.Camera
{
	[Serializable]
	public class HorizontalRotationProcessor : SinCosProcessor
	{
		protected override void ExecuteInternal(Transform transform, float strength)
		{
			Vector3 rotation = transform.localEulerAngles;
			rotation.y = Mathf.Sin(_frequency * _timer) * _amplitude;
			transform.localEulerAngles = rotation;
		}
	}
}