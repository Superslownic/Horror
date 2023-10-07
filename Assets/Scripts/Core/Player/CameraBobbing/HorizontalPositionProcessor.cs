using System;
using UnityEngine;

namespace Scripts.Core.Player
{
	[Serializable]
	public class HorizontalPositionProcessor : SinCosProcessor
	{
		protected override void ExecuteInternal(Transform transform, float strength)
		{
			Vector3 position = transform.localPosition;
			position.x = Mathf.Cos(_frequency * _timer * 0.5f) * _amplitude;
			transform.localPosition = position;
		}
	}
}