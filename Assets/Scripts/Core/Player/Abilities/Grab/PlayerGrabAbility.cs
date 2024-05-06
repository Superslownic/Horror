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
		private UnitFilter _grabbables;
		private UnitFilter _grabbed;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_cursorView = FindObjectOfType<CursorView>();
			_grabbables = UnitFilter.Create().With<GrabbableAbility, SelectedAbility>().Build(Disposable);
			_grabbed = UnitFilter.Create().With<GrabbedAbility>().Build(Disposable);
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			if (_inputManager.Grab.WasPressedThisFrame() && _grabbables.Count > 0)
			{
				foreach (Unit grabableUnit in _grabbables)
				{
					grabableUnit.AddAbility<GrabbedAbility>(x => x.GrabPoint = grabableUnit.GetAbility<SelectedAbility>().HitPoint);
				}

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