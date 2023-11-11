using Scripts.Config;
using Scripts.Entities;
using Scripts.FSM.Composite;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class CrouchingWalkState : State
	{
		[Inject] private readonly Unit _playerUnit;
		[Inject] private readonly GameConfig _gameConfig;
		
		public CrouchingWalkState(string name) : base(name)
		{
		}
		
		public override void Enter()
		{
			_playerUnit.GetAbility<HeadBobAbility>().ReplaceConfig(_gameConfig.Player.Footsteps.CrouchShakeConfig);
			_playerUnit.GetAbility<HeadSwayingAbility>().ReplaceConfig(_gameConfig.Player.Wobble.CrouchWalkShakeConfig);
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
		}
	}
}