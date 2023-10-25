using System;
using DG.Tweening;
using UnityEngine;

namespace Scripts.Config.Player
{
	public abstract class Replaceable<T>
	{
		[field: SerializeField] public T Value { get; set; }
		public Tween Tween { get; set; }

		public void ReplaceValue(T target, float time, Ease ease)
		{
			Tween?.Kill();
			Tween = CreateTween(target, time, ease);
		}
		
		public abstract Tween CreateTween(T target, float time, Ease ease);
	}

	[Serializable]
	public class ReplaceableFloat : Replaceable<float>
	{
		public override Tween CreateTween(float target, float time, Ease ease)
		{
			return DOTween.To(() => Value, value => Value = value, target, time).SetEase(ease);
		}
	}
	
	[Serializable]
	public class ReplaceableVector3 : Replaceable<Vector3>
	{
		public override Tween CreateTween(Vector3 target, float time, Ease ease)
		{
			return DOTween.To(() => Value, value => Value = value, target, time).SetEase(ease);
		}
	}
}