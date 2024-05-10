using System.Collections;
using DG.Tweening;
using Scripts.Units;
using Scripts.Utility.Extensions;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Scripts.Core
{
	public class WaterEnterRipplesAbility : Ability
	{
		[SerializeField] private WaterDeformer _waterRipplesPrefab;
		[SerializeField] private GameObject _waterSplashPrefab;
		[SerializeField] private float _ripplesAmplitude;
		[SerializeField] private float _ripplesMaxAmplitudeSpeed;
		[SerializeField] private float _ripplesStartScale;
		[SerializeField] private float _ripplesEndScale;
		[SerializeField] private float _ripplesDuration;
		[SerializeField] private int _ripplesCount;
		[SerializeField] private float _ripplesDelay;

		private RigidbodyAbility _rigidbodyAbility;
		private CheckWaterAbility _checkWaterAbility;
		private bool _inWater;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_rigidbodyAbility = Unit.GetAbility<RigidbodyAbility>();
			_checkWaterAbility = Unit.GetAbility<CheckWaterAbility>();
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			if (_checkWaterAbility.InWater == _inWater)
				return;

			_inWater = _checkWaterAbility.InWater;

			if(!_inWater)
				return;

			StartCoroutine(SpawnRipples());
		}

		private IEnumerator SpawnRipples()
		{
			for (int i = 0; i < _ripplesCount; i++)
			{
				float speed = _rigidbodyAbility.Rigidbody.linearVelocity.magnitude;
				float multiplier = Mathf.Clamp01(speed / _ripplesMaxAmplitudeSpeed) * (1 - i / (float)_ripplesCount);
				Vector3 ripplesPosition = _rigidbodyAbility.Rigidbody.transform.TransformPoint(_rigidbodyAbility.Rigidbody.centerOfMass).SetY(_checkWaterAbility.WaterSurfaceHeight);
				WaterDeformer ripplesInstance = Instantiate(_waterRipplesPrefab, ripplesPosition, Quaternion.identity);
				GameObject splashInstance = Instantiate(_waterSplashPrefab, ripplesPosition, Quaternion.identity);
				splashInstance.transform.localScale = Vector3.one * (3 * multiplier);

				ripplesInstance.amplitude = _ripplesAmplitude * multiplier;

				Vector3 startScale = ripplesInstance.transform.localScale;
				startScale.x = _ripplesStartScale;
				startScale.y = 1;
				startScale.z = _ripplesStartScale;

				Vector3 endScale = ripplesInstance.transform.localScale;
				endScale.x = _ripplesEndScale * multiplier;
				endScale.y = 1;
				endScale.z = _ripplesEndScale * multiplier;

				ripplesInstance.transform.localScale = startScale;

				DOTween.Sequence()
					.Join(DOTween.To(() => ripplesInstance.amplitude, value => ripplesInstance.amplitude = value, endValue: 0, _ripplesDuration * multiplier))
					.Join(DOTween.To(() => ripplesInstance.transform.localScale, value => ripplesInstance.transform.localScale = value, endValue: endScale, _ripplesDuration * multiplier))
					.SetEase(Ease.OutCubic)
					.AppendCallback(() => Destroy(ripplesInstance.gameObject));

				yield return new WaitForSeconds(_ripplesDelay);
			}
		}
	}
}