using Scripts.Config;
using Scripts.Config.Player;
using Scripts.Entities;
using Scripts.FSM.Composite;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class StandingIdleState : State
	{
		[Inject] private readonly Unit _playerUnit;
		[Inject] private readonly GameConfig _gameConfig;
		
		public StandingIdleState(string name) : base(name)
		{
		}

		public override void Enter()
		{
			_playerUnit.GetAbility<HeadBobAbility>().ReplaceConfig(HeadBobShakeVaraintConfig.Default);
			_playerUnit.GetAbility<HeadSwayingAbility>().ReplaceConfig(_gameConfig.Player.Wobble.StandIdleShakeConfig);
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
		}
	}
}