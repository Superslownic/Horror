using System;
using UnityEngine;

namespace Scripts.Configs.Player
{
	[Serializable]
	public class LadderConfig
	{
		[field: SerializeField] public AnimationCurve DismountCurve { get; private set; }
		[field: SerializeField] public LadderValuesConfig DefaultValues { get; private set; }
		[field: SerializeField] public LadderValuesConfig RunValues { get; private set; }
	}
}