using Scripts.Configs;
using Scripts.TFSM;
using Scripts.Units;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class CrouchWalkState : LeafState<CrouchSuperState>
	{
		[Inject] private readonly Unit _playerUnit;
		[Inject] private readonly GameConfig _gameConfig;

		public override void OnEnter()
		{
			_playerUnit.GetAbility<HeadBobAbility>().ReplaceConfig(_gameConfig.Player.Footsteps.CrouchShakeConfig);
			_playerUnit.GetAbility<HeadSwayAbility>().ReplaceConfig(_gameConfig.Player.Sway.CrouchWalkShakeConfig);
		}
	}
}