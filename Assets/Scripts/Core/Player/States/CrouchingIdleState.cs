using Scripts.Configs;
using Scripts.Configs.Player;
using Scripts.FSM.Composite;
using Scripts.Units;
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
			_playerUnit.GetAbility<HeadSwayAbility>().ReplaceConfig(_gameConfig.Player.Sway.CrouchIdleShakeConfig);
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
		}
	}
}