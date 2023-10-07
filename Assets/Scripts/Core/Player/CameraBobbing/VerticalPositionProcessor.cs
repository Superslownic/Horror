using System;
using UnityEngine;

namespace Scripts.Core.Player
{
	[Serializable]
	public class VerticalPositionProcessor : SinCosProcessor
	{
		protected override void ExecuteInternal(Transform transform, float strength)
		{
			_timer += Time.deltaTime * _frequency * strength;
			Vector3 position = transform.localPosition;
			position.y = Mathf.Sin(_timer) * _amplitude;
			transform.localPosition = position;
		}
	}
}