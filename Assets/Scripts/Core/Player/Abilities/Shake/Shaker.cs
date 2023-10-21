using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.Core.Player.Shake
{
	[Serializable]
	public class Shaker
	{
		[field: SerializeField, EnumToggleButtons, HideLabel]
		public ShakeProcessorType Type { get; set; }
		
		[field: SerializeField, BoxGroup("Start", showLabel: false), HorizontalGroup("Start/Values"), LabelText("Start"), Min(0)]
		public float StartDuration { get; set; }
		
		[field: SerializeField, BoxGroup("Start", showLabel: false), HorizontalGroup("Start/Values"), HideLabel]
		public Ease StartEase { get; set; }
		
		[field: SerializeField, HideIf("Type", ShakeProcessorType.Infinite), BoxGroup("Mid", showLabel: false), LabelText("Mid"), Min(0)]
		public float MidDuration { get; set; }
		
		[field: SerializeField, BoxGroup("Stop", showLabel: false), HorizontalGroup("Stop/Values"), LabelText("Stop"), Min(0)]
		public float StopDuration { get; set; }
		
		[field: SerializeField, BoxGroup("Stop", showLabel: false), HorizontalGroup("Stop/Values"), HideLabel]
		public Ease StopEase { get; set; }
		
		[field: SerializeReference]
		public IShakeProcessor Processor { get; set; }

		public ShakeProcessorState State { get; private set; }
		public float Power { get; private set; }
		
		private float _midTimer;
		private Tween _tween;

		public void Update(out Vector3 position, out Vector3 rotation)
		{
			if (Type == ShakeProcessorType.Finite && State == ShakeProcessorState.Started)
			{
				_midTimer += Time.deltaTime;
					
				if (_midTimer > MidDuration)
				{
					Stop();
				}
			}
			
			Processor.Update(out Vector3 internalPosition, out Vector3 internalRotation);
			
			position = internalPosition * Power;
			rotation = internalRotation * Power;
		}

		[HorizontalGroup("Buttons"), Button(ButtonSizes.Small), GUIColor(0, 1, 0)]
		public void Start()
		{
			if (State is ShakeProcessorState.Starting or ShakeProcessorState.Started)
				return;
			
			State = ShakeProcessorState.Starting;
			_midTimer = 0;
			_tween?.Kill();
			_tween = DOTween
				.To(() => Power, value => Power = value, 1, StartDuration)
				.SetEase(StartEase)
				.OnComplete(() => State = ShakeProcessorState.Started);
		}

		[HorizontalGroup("Buttons"), Button(ButtonSizes.Small), GUIColor(1, 0, 0)]
		public void Stop()
		{
			if (State is ShakeProcessorState.Stopping or ShakeProcessorState.Stopped)
				return;
			
			State = ShakeProcessorState.Stopping;
			_tween?.Kill();
			_tween = DOTween
				.To(() => Power, value => Power = value, 0, StopDuration)
				.SetEase(StopEase)
				.OnComplete(() => State = ShakeProcessorState.Stopped);
		}

		public Shaker Clone()
		{
			return new Shaker
			{
				Type = Type,
				StartDuration = StartDuration,
				StartEase = StartEase,
				MidDuration = MidDuration,
				StopDuration = StopDuration,
				StopEase = StopEase,
				Processor = Processor.Clone()
			};
		}
	}
}