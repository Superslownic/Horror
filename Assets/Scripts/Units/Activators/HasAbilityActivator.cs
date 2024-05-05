using Scripts.Reactive;

namespace Scripts.Units
{
	public abstract class HasAbilityActivator<T> : AbilityActivator
		where T : Ability
	{
		private readonly CompositeDisposable _disposable = new();

		protected override void OnInitialize()
		{
			Unit.AbilityAddedAction.AddListener(_ => UpdateActivation()).AddTo(_disposable);
			Unit.AbilityRemovedAction.AddListener(_ => UpdateActivation()).AddTo(_disposable);
		}

		protected override void OnDispose()
		{
			_disposable.Dispose();
		}

		protected override void UpdateActivation()
		{
			if (Unit.HasAbility<T>())
			{
				Activate();
			}
			else
			{
				Deactivate();
			}
		}
	}
}