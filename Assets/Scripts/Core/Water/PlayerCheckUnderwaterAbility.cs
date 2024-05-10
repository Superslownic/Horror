using Scripts.Core.Player;
using Scripts.Reactive;
using Scripts.Units;
using UnityEngine;

namespace Scripts.Core
{
	public class PlayerCheckUnderwaterAbility : Ability
	{
		public DisposableAction EnterAction { get; } = new();
		public DisposableAction ExitAction { get; } = new();

		public bool IsUnderWater { get; private set; }

		private PlayerHeadAbility _playerHeadAbility;
		private PlayerCheckWaterAbility _checkWaterAbility;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();
			_checkWaterAbility = Unit.GetAbility<PlayerCheckWaterAbility>();
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			bool isUnderWater = _checkWaterAbility.IsInWater && _playerHeadAbility.HeadStaticAnchor.position.y < _checkWaterAbility.WaterSurfaceHeight;

			if (IsUnderWater == isUnderWater)
				return;

			IsUnderWater = isUnderWater;

			if (IsUnderWater)
			{
				EnterAction.Invoke();
			}
			else
			{
				ExitAction.Invoke();
			}
		}
	}
}