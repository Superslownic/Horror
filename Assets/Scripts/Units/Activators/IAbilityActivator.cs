using Scripts.Reactive;

namespace Scripts.Units
{
	public interface IAbilityActivator
	{
		DisposableAction OnStateChanged { get; }
		AbilityActivatorTarget Target { get; set; }
		Ability Ability { get; set; }
		bool State { get; }
		void Initialize();
	}
}