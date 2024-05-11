using System;
using Scripts.Configs;
using Scripts.TFSM;
using Scripts.Units;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class StandSuperState : SuperState
	{
		[Inject] private readonly Unit _playerUnit;
		[Inject] private readonly GameConfig _gameConfig;

		public override Type Parent => null;

		public override void OnEnter()
		{
			_playerUnit.GetAbility<ChangeVelocityAbility>().AffectGravity = false;
			_playerUnit.GetAbility<GroundMoveAbility>().AddActivator(this);
			_playerUnit.GetAbility<ChangeHeightAbility>().Execute(_gameConfig.Player.ChangeHeight.StandConfig, _gameConfig.Player.ChangeHeight.Duration);
			_playerUnit.GetAbility<HeadBobAbility>().AddActivator(this);
		}

		public override void OnExit()
		{
			_playerUnit.GetAbility<HeadBobAbility>().RemoveActivator(this);
			_playerUnit.GetAbility<GroundMoveAbility>().RemoveActivator(this);
		}
	}
}