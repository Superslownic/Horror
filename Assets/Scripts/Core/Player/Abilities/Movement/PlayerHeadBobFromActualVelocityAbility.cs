using Scripts.Units;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class PlayerHeadBobFromActualVelocityAbility : Ability
	{
		private GroundMoveAbility _groundMoveAbility;
		private ChangeVelocityAbility _changeVelocityAbility;
		private HeadBobAbility _headBobAbility;

		protected override void OnInitialize()
		{
			_groundMoveAbility = Unit.GetAbility<GroundMoveAbility>();
			_changeVelocityAbility = Unit.GetAbility<ChangeVelocityAbility>();
			_headBobAbility = Unit.GetAbility<HeadBobAbility>();
		}

		protected override void OnUpdate()
		{
			_headBobAbility.Magnitude = _groundMoveAbility.IsGrounded && _groundMoveAbility.IsMoving
				? _changeVelocityAbility.ActualVelocity.normalized.magnitude
				: Mathf.Lerp(_headBobAbility.Magnitude, 0, Time.deltaTime);
		}
	}
}