using Scripts.Behaviour;
using Scripts.Reactive;

namespace Scripts.Units
{
	public abstract class Ability : Toggleable
	{
		public static DisposableAction<Ability> InitializedAction { get; } = new();
		public static DisposableAction<Ability> DisposedAction { get; } = new();

		public Unit Unit { get; private set; }

		protected CompositeDisposable Disposable => _disposable ??= new CompositeDisposable();

		private CompositeDisposable _disposable;

		public void Initialize(Unit unit)
		{
			Unit = unit;
			OnInitialize();
			InitializedAction.Invoke(this);
		}

		private void OnDestroy()
		{
			OnDispose();
			Disposable.Dispose();
			DisposedAction.Invoke(this);
		}

		protected virtual void OnInitialize() { }
		protected virtual void OnDispose() { }
	}
}