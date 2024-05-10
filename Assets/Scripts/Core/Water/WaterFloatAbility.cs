using Scripts.Units;
using UnityEngine;

namespace Scripts.Core
{
	public class WaterFloatAbility : Ability
	{
		[SerializeField] private float _startBuoyancyDepth;
		[SerializeField] private float _buoyancyForce;
		[SerializeField] private Transform[] _floatPoints;
		[SerializeField] private float _linearDamping;
		[SerializeField] private float _angularDamping;

		private RigidbodyAbility _rigidbodyAbility;
		private CheckWaterAbility _checkWaterAbility;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_rigidbodyAbility = Unit.GetAbility<RigidbodyAbility>();
			_checkWaterAbility = Unit.GetAbility<CheckWaterAbility>();
		}

		protected override void OnFixedUpdate()
		{
			base.OnFixedUpdate();

			if(!_checkWaterAbility.InWater)
				return;

			foreach (Transform floatPoint in _floatPoints)
			{
				_rigidbodyAbility.Rigidbody.AddForceAtPosition(Physics.gravity / _floatPoints.Length, floatPoint.position, ForceMode.Acceleration);

				if (floatPoint.position.y >= _checkWaterAbility.WaterSurfaceHeight)
					continue;

				float force = Mathf.Clamp01((_checkWaterAbility.WaterSurfaceHeight - floatPoint.position.y) / _startBuoyancyDepth) * _buoyancyForce;
				_rigidbodyAbility.Rigidbody.AddForceAtPosition(new Vector3(0, Mathf.Abs(Physics.gravity.y) * force, 0), floatPoint.position, ForceMode.Acceleration);
				_rigidbodyAbility.Rigidbody.AddForce(-_rigidbodyAbility.Rigidbody.linearVelocity * (force * _linearDamping * Time.fixedDeltaTime), ForceMode.VelocityChange);
				_rigidbodyAbility.Rigidbody.AddTorque(-_rigidbodyAbility.Rigidbody.angularVelocity * (force * _angularDamping * Time.fixedDeltaTime), ForceMode.VelocityChange);
			}
		}
	}
}