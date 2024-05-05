using Scripts.Input;
using Scripts.UI;
using Scripts.Units;
using Zenject;

namespace Scripts.Core.Player
{
	public class PlayerGrabAbility : Ability
	{
		[Inject] private readonly InputManager _inputManager;

		private CursorView _cursorView;
		private UnitFilter _grabables;
		private UnitFilter _grabbed;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_cursorView = FindObjectOfType<CursorView>();
			_grabables = UnitFilter.Create().With<GrabableAbility, SelectedAbility>().Build(Disposable);
			_grabbed = UnitFilter.Create().With<GrabbedAbility>().Build(Disposable);
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			if (_inputManager.Grab.WasPressedThisFrame() && _grabables.Count > 0)
			{
				foreach (Unit grabableUnit in _grabables)
					grabableUnit.AddAbility<GrabbedAbility>();

				_cursorView.gameObject.SetActive(false);
			}

			if (_inputManager.Grab.WasReleasedThisFrame() && _grabbed.Count > 0)
			{
				foreach (Unit grabbedUnit in _grabbed)
					grabbedUnit.RemoveAbility<GrabbedAbility>();

				_cursorView.gameObject.SetActive(true);
			}
		}
	}
}