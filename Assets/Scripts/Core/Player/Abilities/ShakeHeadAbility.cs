using Scripts.Entities;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class ShakeHeadAbility : Ability
	{
		[field: SerializeField] public Shaker Shaker { get; private set; }

		protected override void OnActivate()
		{
			Shaker.enabled = true;
		}

		protected override void OnDeactivate()
		{
			Shaker.enabled = false;
		}
	}
}