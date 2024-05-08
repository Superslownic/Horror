using Scripts.Config;
using Scripts.Config.Player;
using Scripts.FSM.Composite;
using Scripts.Units;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class SwimmingMoveState : State
	{
		[Inject] private readonly Unit _playerUnit;
		[Inject] private readonly GameConfig _gameConfig;

		public SwimmingMoveState(string name) : base(name)
		{
		}

		public override void Enter()
		{
			_playerUnit.GetAbility<HeadBobAbility>().ReplaceConfig(HeadBobShakeVaraintConfig.Default);
			_playerUnit.GetAbility<HeadSwayingAbility>().ReplaceConfig(_gameConfig.Player.Wobble.SwimMoveShakeConfig);
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
		}
	}
}