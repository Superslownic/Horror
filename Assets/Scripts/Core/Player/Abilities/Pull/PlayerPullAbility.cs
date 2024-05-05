using Scripts.Reactive;
using Scripts.Units;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class PlayerPullAbility : Ability
	{
		[SerializeField] private float _forceMultiplier;
		[SerializeField] private float _maxForce;
		[SerializeField] private float _maxMass;

		private PlayerHeadAbility _playerHeadAbility;
		private UnitFilter _pullables;
		private Transform _pullAnchor;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();
			_pullables = UnitFilter.Create().With<GrabbedAbility, PullableAbility>().Build(Disposable);
			_pullables.AddedAction.AddListener(HandlePullableAdded).AddTo(Disposable);
			CreateAnchor();
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			if(_pullables.Count == 0)
				return;

			foreach (Unit pullableUnit in _pullables)
			{
				Rigidbody pullableRigidbody = pullableUnit.GetAbility<RigidbodyAbility>();
				Vector3 direction = _pullAnchor.position - pullableUnit.transform.position;
				direction *= _forceMultiplier / (Mathf.Max(pullableRigidbody.mass, 2) * 0.5f);
				direction = Vector3.ClampMagnitude(direction, _maxForce);
				bool canLift = pullableRigidbody.mass < _maxMass;
				pullableRigidbody.velocity = canLift
					? direction
					: new Vector3(direction.x, pullableRigidbody.velocity.y, direction.z);
			}
		}

		private void HandlePullableAdded(Unit unit)
		{
			unit.GetAbility<RigidbodyAbility>().Rigidbody.velocity = Vector3.zero;
			unit.GetAbility<RigidbodyAbility>().Rigidbody.angularVelocity = Vector3.zero;
			_pullAnchor.position = unit.transform.position;
			_pullAnchor.rotation = unit.transform.rotation;
		}

		private void CreateAnchor()
		{
			_pullAnchor = new GameObject("Pull Anchor").transform;
			_pullAnchor.SetParent(_playerHeadAbility.HeadFloatingAnchor);
			_pullAnchor.localPosition = Vector3.zero;
		}
	}
}