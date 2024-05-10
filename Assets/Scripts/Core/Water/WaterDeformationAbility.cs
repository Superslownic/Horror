using Scripts.Units;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Scripts.Core
{
	public class WaterDeformationAbility : Ability
	{
		[SerializeField] private WaterDeformer _waterDeformer;
		[SerializeField] private float _maxVelocity;
		[SerializeField] private float _maxAmplitude;

		private RigidbodyAbility _rigidbodyAbility;
		private CheckWaterAbility _checkWaterAbility;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_rigidbodyAbility = Unit.GetAbility<RigidbodyAbility>();
			_checkWaterAbility = Unit.GetAbility<CheckWaterAbility>();
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			_waterDeformer.amplitude = _checkWaterAbility.IsInWater
				? Mathf.Clamp01(_rigidbodyAbility.Rigidbody.linearVelocity.magnitude / _maxVelocity) * _maxAmplitude
				: 0;
		}
	}
}