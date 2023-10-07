using System;
using UnityEngine;

namespace Scripts.Core.Player
{
	[Serializable]
	public class VerticalRotationProcessor : SinCosProcessor
	{
		protected override void ExecuteInternal(Transform transform, float strength)
		{
			Vector3 rotation = transform.localEulerAngles;
			rotation.x = Mathf.Cos(_frequency * _timer * 0.5f) * _amplitude;
			transform.localEulerAngles = rotation;
		}
	}
}