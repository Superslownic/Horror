using Scripts.Input;
using Scripts.Units;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class WaterMoveAbility : Ability
	{
		[SerializeField] private float _speed;

		[Inject] private readonly InputManager _inputManager;

		private PlayerHeadAbility _playerHeadAbility;
		private ChangeVelocityAbility _changeVelocityAbility;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();
			_changeVelocityAbility = Unit.GetAbility<ChangeVelocityAbility>();
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			Vector2 moveInput = _inputManager.Move.ReadValue<Vector2>();
			Vector3 forwardInputMotion = _playerHeadAbility.HeadDetachedAnchor.forward * moveInput.y;
			Vector3 sideInputMotion = _playerHeadAbility.HeadDetachedAnchor.right * moveInput.x;
			Vector3 resultInputMotion = (forwardInputMotion + sideInputMotion) * _speed;
			Vector3 clampedInputMotion = Vector3.ClampMagnitude(resultInputMotion, _speed);

			_changeVelocityAbility.SetTargetVelocity(clampedInputMotion);
		}
	}
}