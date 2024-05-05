using System.Linq;
using Scripts.UI;
using Scripts.Units;
using Scripts.Utility.Extensions;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class PlayerSelectAbility : Ability
	{
		[SerializeField] private float _maxDistance;
		[SerializeField] private LayerMask _layerMask;

		private PlayerHeadAbility _playerHead;
		private UnitFilter _selected;
		private CursorView _cursorView;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_playerHead = Unit.GetAbility<PlayerHeadAbility>();
			_selected = UnitFilter.Create().With<SelectedAbility>().Build(Disposable);
			_cursorView = FindObjectOfType<CursorView>();
		}

		protected override void OnUpdate()
		{
			Ray ray = _playerHead.Camera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));

			if (Physics.Raycast(ray, out RaycastHit hitInfo, _maxDistance, _layerMask))
			{
				if (!hitInfo.transform.TryGetUnit(out Unit unit))
				{
					Deselect();
					return;
				}

				if (!unit.HasAbility<SelectableAbility>())
				{
					Deselect();
					return;
				}

				if (unit.HasAbility<SelectedAbility>())
					return;

				if(_selected.Count > 0)
					_selected.First().RemoveAbility<SelectedAbility>();

				unit.AddAbility<SelectedAbility>();
				_cursorView.CenterImage.enabled = true;
			}
			else if(_selected.Count > 0)
			{
				Deselect();
			}
		}

		private void Deselect()
		{
			if(_selected.Count > 0)
				_selected.First().RemoveAbility<SelectedAbility>();

			_cursorView.CenterImage.enabled = false;
		}
	}
}