using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.Core.Player.Shake
{
	[Serializable]
	public class ShakerConfig
	{
		[field: SerializeField]
		public ShakeProcessorType Type { get; set; }
		
		[field: SerializeField, Min(0)]
		public float StartDuration { get; set; }
		
		[field: SerializeField]
		public Ease StartEase { get; set; }
		
		[field: SerializeField, HideIf("Type", ShakeProcessorType.Infinite), Min(0)]	
		public float MidDuration { get; set; }
		
		[field: SerializeField, Min(0)]
		public float StopDuration { get; set; }
		
		[field: SerializeField]
		public Ease StopEase { get; set; }
	}
}