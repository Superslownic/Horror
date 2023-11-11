using System;
using UnityEngine;

namespace Scripts.Config.Player
{
	[Serializable]
	public class LadderConfig
	{
		[field: SerializeField] public AnimationCurve DismountCurve { get; private set; }
		[field: SerializeField] public LadderValuesConfig DefaultValues { get; private set; }
		[field: SerializeField] public LadderValuesConfig RunValues { get; private set; }
	}
}