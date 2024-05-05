using Scripts.Reactive;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.Units
{
	[DefaultExecutionOrder(3)]
	public abstract class AbilityActivator : MonoBehaviour, IAbilityActivator
	{
		public DisposableAction OnStateChanged { get; } = new();

		[field: SerializeField, EnumToggleButtons]
		public AbilityActivatorTarget Target { get; set; }

		[field: SerializeField, ShowIf(nameof(Target), AbilityActivatorTarget.Manual)]
		public Ability Ability { get; set; }
		
		[SerializeField] private bool _inverse;

		public Unit Unit => Ability.Unit;
		public bool State { get; private set; }

		private void Awake()
		{
			if (Target != AbilityActivatorTarget.Auto)
			{
				Initialize();
			}
		}

		private void OnDestroy()
		{
			OnDispose();
		}

		public void Initialize()
		{
			switch (Target)
			{
				case AbilityActivatorTarget.Parent:
				{
					Ability = GetComponentInParent<Ability>();
					break;
				}

				case AbilityActivatorTarget.Self:
				{
					Ability = GetComponent<Ability>();
					break;
				}
			}

			OnInitialize();
			UpdateActivation();
		}

		protected abstract void OnInitialize();
		protected abstract void UpdateActivation();
		protected abstract void OnDispose();

		protected void Activate()
		{
			State = true ^ _inverse;

			if (Target == AbilityActivatorTarget.Auto)
			{
				OnStateChanged.Invoke();
			}
			else
			{
				Ability.RemoveDeactivator(this);
			}
		}

		protected void Deactivate()
		{
			State = false ^ _inverse;

			if (Target == AbilityActivatorTarget.Auto)
			{
				OnStateChanged.Invoke();
			}
			else
			{
				Ability.AddDeactivator(this);
			}
		}
	}
}