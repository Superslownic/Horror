using Scripts.Units;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class AttachHeadAbility : Ability
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

		/*protected override void OnFixedUpdate()
		{
			_playerHeadAbility.HeadDetachedAnchorRigidbody.MovePosition(_playerHeadAbility.HeadStaticAnchor.position);
		}*/
	}
}