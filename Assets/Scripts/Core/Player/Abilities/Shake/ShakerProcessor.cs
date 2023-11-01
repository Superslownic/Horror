using System;
using DG.Tweening;
using UnityEngine;

namespace Scripts.Core.Player
{
	[Serializable]
	public class ShakerProcessor
	{
		[field: SerializeField]
		public ShakerConfig Config { get; set; }
		
		[field: SerializeReference]
		public IShakeVariant Variant { get; set; }

		public ShakeProcessorState State { get; private set; }
		public float Power { get; private set; }
		
		private float _midTimer;
		private Tween _tween;

		public void Update(out Vector3 position, out Vector3 rotation)
		{
			if (Config.Type == ShakeProcessorType.Finite && State == ShakeProcessorState.Started)
			{
				_midTimer += Time.deltaTime;
					
				if (_midTimer >= Config.MidDuration)
				{
					Stop();
				}
			}
			
			Variant.Update(out Vector3 internalPosition, out Vector3 internalRotation);
			
			position = internalPosition * Power;
			rotation = internalRotation * Power;
		}

		public void Start()
		{
			if (State is ShakeProcessorState.Starting or ShakeProcessorState.Started)
				return;

			State = ShakeProcessorState.Starting;
			Variant.Reset();
			Power = 0;
			_midTimer = 0;
			
			_tween?.Kill();
			_tween = DOTween
				.To(() => Power, value => Power = value, 1, Config.StartDuration)
				.SetEase(Config.StartEase)
				.OnComplete(() => State = ShakeProcessorState.Started);
		}

		public void Stop()
		{
			if (State is ShakeProcessorState.Stopping or ShakeProcessorState.Stopped)
				return;
			
			State = ShakeProcessorState.Stopping;
			
			_tween?.Kill();
			_tween = DOTween
				.To(() => Power, value => Power = value, 0, Config.StopDuration)
				.SetEase(Config.StopEase)
				.OnComplete(() => State = ShakeProcessorState.Stopped);
		}
	}
}