using System;
using UnityEngine;

namespace Scripts.Config.Camera
{
	[Serializable]
	public class CameraConfig
	{
		[field: SerializeField] public CameraLeanConfig Lean { get; private set; }
		[field: SerializeField] public CameraSmoothConfig Smooth { get; private set; }
		[field: SerializeField] public CameraBobbingConfig Bobbing { get; private set; }
	}
}