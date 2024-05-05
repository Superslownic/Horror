using Scripts.Config;
using Scripts.FSM.Composite;
using Scripts.Units;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class CrouchingSuperState : SuperState
	{
		[Inject] private readonly Unit _playerUnit;
		[Inject] private readonly GameConfig _gameConfig;
		
		public CrouchingSuperState(string name, State initialState) : base(name, initialState)
		{
		}

		public override void Enter()
		{
			_playerUnit.GetAbility<CrouchAbility>().PerformCrouch();
			_playerUnit.GetAbility<MovementAbility>().ReplaceConfig(_gameConfig.Player.Movement.Crouching);
			base.Enter();
		}

		public override void Exit()
		{
			_playerUnit.GetAbility<CrouchAbility>().PerformStand();
			base.Exit();
		}
	}
}