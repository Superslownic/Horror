using System;
using Scripts.Core.Player;
using UnityEngine;

namespace Scripts.Configs.Player
{
	[Serializable]
	public class HeadBobConfig
	{
		[field: SerializeField] public float ChangeValuesDuration { get; private set; }
		[field: SerializeField] public ShakerConfig ShakerConfig { get; private set; }
		[field: SerializeField] public HeadBobShakeVaraintConfig WalkShakeConfig { get; private set; }
		[field: SerializeField] public HeadBobShakeVaraintConfig RunShakeConfig { get; private set; }
		[field: SerializeField] public HeadBobShakeVaraintConfig CrouchShakeConfig { get; private set; }
	}
}