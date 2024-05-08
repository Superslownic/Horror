using Scripts.Reactive;
using Scripts.Units;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class CheckWater : Ability
	{
		[ShowInInspector] public bool InWater => IsActive;

		[SerializeField] private TriggerProvider _trigger;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_trigger.OnEnter.AddListener(HandleHeadTriggerEnter).AddTo(Disposable);
			_trigger.OnExit.AddListener(HandleHeadTriggerExit).AddTo(Disposable);
		}

		private void HandleHeadTriggerEnter(Unit unit)
		{
			if (unit.HasAbility<WaterMarkerAbility>())
			{
				AddActivator(unit);
			}
		}

		private void HandleHeadTriggerExit(Unit unit)
		{
			if (unit.HasAbility<WaterMarkerAbility>())
			{
				RemoveActivator(unit);
			}
		}
	}
}