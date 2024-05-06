using System.Linq;
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
		private (Rigidbody rigidbody, ConfigurableJoint configurableJoint) _grabAnchor;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();
			_pullables = UnitFilter.Create().With<GrabbedAbility, PullableAbility>().Build(Disposable);
			_pullables.AddedAction.AddListener(HandlePullableAdded).AddTo(Disposable);
			_pullables.RemovedAction.AddListener(HandlePullableRemoved).AddTo(Disposable);
			CreateAnchor();
			CreateJoint();
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			if(_pullables.Count == 0)
				return;

			foreach (Unit pullableUnit in _pullables)
			{
				//Rigidbody pullableRigidbody = pullableUnit.GetAbility<RigidbodyAbility>();
				//Vector3 direction = _pullAnchor.position - pullableUnit.transform.position;

				//pullableRigidbody.automaticCenterOfMass = false;
				//pullableRigidbody.centerOfMass = pullableRigidbody.transform.InverseTransformPoint(pullableUnit.GetAbility<GrabbedAbility>().GrabPoint);

				//direction *= _forceMultiplier / (Mathf.Max(pullableRigidbody.mass, 2) * 0.5f);
				//direction = Vector3.ClampMagnitude(direction, _maxForce);
				//bool canLift = pullableRigidbody.mass < _maxMass;
				//pullableRigidbody.linearVelocity = canLift
				//	? direction
				//	: new Vector3(direction.x, pullableRigidbody.linearVelocity.y, direction.z);

			}

			/*Vector3 direction = _pullAnchor.position - _grabAnchor.rigidbody.transform.position;

			direction *= _forceMultiplier / (Mathf.Max(_grabAnchor.rigidbody.mass, 2) * 0.5f);
			direction = Vector3.ClampMagnitude(direction, _maxForce);
			bool canLift = _grabAnchor.rigidbody.mass < _maxMass;
			_grabAnchor.rigidbody.linearVelocity = canLift
				? direction
				: new Vector3(direction.x, _grabAnchor.rigidbody.linearVelocity.y, direction.z);*/

			Rigidbody pullableRigidbody = _pullables.First().GetAbility<RigidbodyAbility>();
			Vector3 direction = _pullAnchor.position - _grabAnchor.rigidbody.transform.position;

			if(direction.magnitude > 1)
				direction = direction.normalized;

			direction *= _forceMultiplier / (Mathf.Max(pullableRigidbody.mass, 2) * 0.5f);
			direction = Vector3.ClampMagnitude(direction, _maxForce);
			bool canLift = pullableRigidbody.mass < _maxMass;

			_grabAnchor.rigidbody.linearVelocity = canLift
				? direction
				: new Vector3(direction.x, pullableRigidbody.linearVelocity.y, direction.z);

			Debug.DrawLine(_grabAnchor.rigidbody.transform.position, _pullAnchor.position);
		}

		private void HandlePullableAdded(Unit unit)
		{
			Rigidbody pullableRigidbody = unit.GetAbility<RigidbodyAbility>();
			pullableRigidbody.linearVelocity = Vector3.zero;
			pullableRigidbody.angularVelocity = Vector3.zero;
			_pullAnchor.position = unit.GetAbility<GrabbedAbility>().GrabPoint;
			_grabAnchor.rigidbody.transform.position = _pullAnchor.position;
			_grabAnchor.configurableJoint.connectedBody = pullableRigidbody;
		}

		private void HandlePullableRemoved(Unit unit)
		{
			_grabAnchor.configurableJoint.connectedBody = null;
		}

		private void CreateAnchor()
		{
			_pullAnchor = new GameObject("Pull Anchor").transform;
			_pullAnchor.SetParent(_playerHeadAbility.HeadFloatingAnchor);
			_pullAnchor.localPosition = Vector3.zero;
		}

		private void CreateJoint()
		{
			GameObject go = new GameObject("Grab Anchor");

			Rigidbody rigidbody = go.AddComponent<Rigidbody>();
			rigidbody.useGravity = false;
			rigidbody.mass = 1;

			ConfigurableJoint configurableJoint = go.AddComponent<ConfigurableJoint>();
			configurableJoint.xMotion = ConfigurableJointMotion.Locked;
			configurableJoint.yMotion = ConfigurableJointMotion.Locked;
			configurableJoint.zMotion = ConfigurableJointMotion.Locked;

			_grabAnchor = (rigidbody, configurableJoint);
		}
	}
}