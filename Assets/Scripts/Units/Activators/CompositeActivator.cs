using System.Collections.Generic;
using Scripts.Reactive;
using Scripts.Utility.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.Units
{
	[DefaultExecutionOrder(3)]
	public abstract class CompositeActivator : MonoBehaviour, IAbilityActivator
	{
		public DisposableAction OnStateChanged { get; } = new();

		[field: SerializeField, EnumToggleButtons]
		public AbilityActivatorTarget Target { get; set; }

		[field: SerializeField, ShowIf(nameof(Target), AbilityActivatorTarget.Manual)]
		public Ability Ability { get; set; }

		public abstract bool State { get; }

		protected readonly List<IAbilityActivator> _activators = new();

		private readonly CompositeDisposable _disposable = new();

		private void Awake()
		{
			_activators.AddRange(GetComponents<IAbilityActivator>());
			_activators.Remove(this);
			_activators.AddRange(this.GetComponentsInClosestChildren<IAbilityActivator>());

			if (Target != AbilityActivatorTarget.Auto)
				Initialize();
		}

		private void OnDestroy()
		{
			_disposable.Dispose();
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

			foreach (IAbilityActivator activator in _activators)
			{
				activator.Target = AbilityActivatorTarget.Auto;
				activator.Ability = Ability;
				activator.OnStateChanged.AddListener(OnStateChanged.Invoke).AddTo(_disposable);
				activator.Initialize();
			}

			OnStateChanged.AddListener(UpdateActivation).WithInvoke().AddTo(_disposable);
		}
		
		public void UpdateActivation()
		{
			if (State)
			{
				Ability.RemoveDeactivator(this);
			}
			else
			{
				Ability.AddDeactivator(this);
			}
		}
	}
}