using Scripts.Config.Player;
using Scripts.Units;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class ChangeVelocityAbility : Ability
	{
		public Vector3 TargetVelocity => _targetVelocity;
		public Vector3 ActualVelocity => _actualVelocity;
		public bool AffectGravity { get; set; } = true;
		public TweenableFloat ChangeSpeed { get; } = new();

		private RigidbodyAbility _rigidbodyAbility;
		private Vector3 _targetVelocity;
		private Vector3 _actualVelocity;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_rigidbodyAbility = Unit.GetAbility<RigidbodyAbility>();
		}

		protected override void OnFixedUpdate()
		{
			base.OnFixedUpdate();

			_actualVelocity = Vector3.Lerp(_actualVelocity, _targetVelocity, ChangeSpeed * Time.fixedDeltaTime);

			if (!AffectGravity)
				_actualVelocity.y = _rigidbodyAbility.Rigidbody.linearVelocity.y;

			_rigidbodyAbility.Rigidbody.linearVelocity = _actualVelocity;
		}

		public void SetTargetVelocity(float x = float.NaN, float y = float.NaN, float z = float.NaN)
		{
			if (!float.IsNaN(x))
			{
				_targetVelocity.x = x;
			}

			if (!float.IsNaN(y))
			{
				_targetVelocity.y = y;
			}

			if (!float.IsNaN(z))
			{
				_targetVelocity.z = z;
			}
		}

		public void SetTargetVelocity(Vector3 value)
		{
			_targetVelocity = value;
		}

		public void SetActualVelocity(float x = float.NaN, float y = float.NaN, float z = float.NaN)
		{
			if (!float.IsNaN(x))
			{
				_actualVelocity.x = x;
				_targetVelocity.x = x;
			}

			if (!float.IsNaN(y))
			{
				_actualVelocity.y = y;
				_targetVelocity.y = y;
			}

			if (!float.IsNaN(z))
			{
				_actualVelocity.z = z;
				_targetVelocity.z = z;
			}

			_rigidbodyAbility.Rigidbody.linearVelocity = _actualVelocity;
		}

		public void SetActualVelocity(Vector3 value)
		{
			_actualVelocity = value;
			_targetVelocity = value;
			_rigidbodyAbility.Rigidbody.linearVelocity = value;
		}
	}
}