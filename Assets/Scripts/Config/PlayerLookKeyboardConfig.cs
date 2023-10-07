using System;
using UnityEngine;

namespace Scripts.Config
{
	[Serializable]
	public class PlayerLookKeyboardConfig
	{
		[field: SerializeField] public float Sensitivity { get; private set; }
	}
}