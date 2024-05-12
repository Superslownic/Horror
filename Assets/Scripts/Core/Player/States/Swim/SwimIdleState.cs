using Scripts.Configs;
using Scripts.Configs.Player;
using Scripts.TFSM;
using Scripts.Units;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class SwimIdleState : LeafState<SwimSuperState>
	{
		[Inject] private readonly Unit _playerUnit;
		[Inject] private readonly GameConfig _gameConfig;

		public override void OnEnter()
		{
			_playerUnit.GetAbility<HeadBobAbility>().ReplaceConfig(HeadBobShakeVaraintConfig.Default);
			_playerUnit.GetAbility<HeadSwayAbility>().ReplaceConfig(_gameConfig.Player.Sway.SwimIdleShakeConfig);
		}
	}
}