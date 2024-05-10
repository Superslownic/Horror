using Scripts.Reactive;
using Scripts.Units;
using UnityEngine;

namespace Scripts.Core.Player.Breath
{
	public class PlayerOxygenAbility : Ability
	{
		public DisposableAction AmountChangedAction { get; } = new();
		public DisposableAction AmountFullAction { get; } = new();

		public bool IsFull => CurrentAmount == MaxAmount;

		[field: SerializeField] public float MaxAmount { get; private set; }
		[field: SerializeField] public float CurrentAmount { get; private set; }

		[SerializeField] private float _spendSpeed;
		[SerializeField] private float _gainSpeed;
		[SerializeField] private float _screenFadeStartValue;

		private PlayerCheckUnderwaterAbility _checkUnderwaterAbility;
		private PlayerScreenFadeAbility _screenFadeAbility;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_checkUnderwaterAbility = Unit.GetAbility<PlayerCheckUnderwaterAbility>();
			_screenFadeAbility = Unit.GetAbility<PlayerScreenFadeAbility>();
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			float lastAmount = CurrentAmount;

			if (_checkUnderwaterAbility.IsUnderWater)
			{
				CurrentAmount -= _spendSpeed * Time.deltaTime;
			}
			else
			{
				CurrentAmount += _gainSpeed * Time.deltaTime;
			}

			CurrentAmount = Mathf.Clamp(CurrentAmount, 0, MaxAmount);

			if(lastAmount != CurrentAmount)
			{
				_screenFadeAbility.SetValue(1 - Mathf.Clamp01(CurrentAmount / _screenFadeStartValue));
				AmountChangedAction.Invoke();

				if (IsFull)
				{
					AmountFullAction.Invoke();
				}
			}
		}
	}
}