using Scripts.Units;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class SmoothHeadPositionAbility : Ability
	{
		[SerializeField] private Follower _follower;

		protected override void OnActivate()
		{
			base.OnActivate();
			_follower.RemoveDeactivator(this);
		}

		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			_follower.AddDeactivator(this);
		}
	}
}