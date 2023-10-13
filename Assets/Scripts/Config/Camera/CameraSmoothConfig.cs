using System;
using UnityEngine;

namespace Scripts.Config.Camera
{
	[Serializable]
	public class CameraSmoothConfig
	{
		[field: SerializeField] public float Force { get; private set; }
	}
}