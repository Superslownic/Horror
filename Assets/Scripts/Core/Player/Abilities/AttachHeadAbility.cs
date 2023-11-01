using Scripts.Entities;

namespace Scripts.Core.Player
{
	public class AttachHeadAbility : Ability, IDeactivatableAbility
	{
		private PlayerHeadAbility _playerHeadAbility;

		protected override void OnInitialize()
		{
			_playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();
		}

		protected override void OnLateUpdate()
		{
			_playerHeadAbility.HeadDetachedAnchor.position = _playerHeadAbility.HeadStaticAnchor.position;
		}
	}
}