using System;
using UnityEngine;

namespace Scripts.Config.Player
{
	[Serializable]
	public class FlashlightConfig
	{
		[field: SerializeField] public float InterpolationSpeed { get; private set; }
		[field: SerializeField] public LayerMask LayerMask { get; private set; }
	}
}