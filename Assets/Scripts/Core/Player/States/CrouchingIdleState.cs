using Scripts.Config;
using Scripts.Config.Player;
using Scripts.Entities;
using Scripts.FSM.Composite;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class CrouchingIdleState : State
	{
		[Inject] private readonly Unit _playerUnit;
		[Inject] private readonly GameConfig _gameConfig;
		
		public CrouchingIdleState(string name) : base(name)
		{
		}

		public override void Enter()
		{
			_playerUnit.GetAbility<HeadBobAbility>().ReplaceConfig(HeadBobShakeVaraintConfig.Default);
			_playerUnit.GetAbility<HeadSwayingAbility>().ReplaceConfig(_gameConfig.Player.Wobble.CrouchIdleShakeConfig);
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
		}
	}
}