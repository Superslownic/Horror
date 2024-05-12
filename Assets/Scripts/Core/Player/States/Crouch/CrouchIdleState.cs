using Scripts.Configs;
using Scripts.Configs.Player;
using Scripts.TFSM;
using Scripts.Units;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class CrouchIdleState : LeafState<CrouchSuperState>
	{
		[Inject] private readonly Unit _playerUnit;
		[Inject] private readonly GameConfig _gameConfig;

		public override void OnEnter()
		{
			_playerUnit.GetAbility<JumpAbility>().AddDeactivator(this);
			_playerUnit.GetAbility<HeadBobAbility>().ReplaceConfig(HeadBobShakeVaraintConfig.Default);
			_playerUnit.GetAbility<HeadSwayAbility>().ReplaceConfig(_gameConfig.Player.Sway.CrouchIdleShakeConfig);
		}

		public override void OnExit()
		{
			_playerUnit.GetAbility<JumpAbility>().RemoveDeactivator(this);
		}
	}
}