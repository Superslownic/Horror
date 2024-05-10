using Scripts.Units;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Scripts.Core
{
	public class WaterFoamAbility : Ability
	{
		[SerializeField] private WaterFoamGenerator _waterFoamGenerator;
		[SerializeField] private float _maxFoamVelocity;

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
			_waterFoamGenerator.surfaceFoamDimmer = _waterFoamGenerator.deepFoamDimmer = _checkWaterAbility.InWater
				? Mathf.Clamp01(_rigidbodyAbility.Rigidbody.linearVelocity.magnitude / _maxFoamVelocity)
				: 0;
		}
	}
}