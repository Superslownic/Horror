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
		[SerializeField] private float _maxThrowForce;
		[SerializeField] private bool _useJoint;

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

			Rigidbody pullableRigidbody = _pullables.First().GetAbility<RigidbodyAbility>();
			Rigidbody targetRigidbody = _useJoint ? _grabAnchor.rigidbody : pullableRigidbody;
			Vector3 direction = _pullAnchor.position - targetRigidbody.transform.position;

			if(direction.magnitude > 1)
				direction = direction.normalized;

			direction *= _forceMultiplier / (Mathf.Max(pullableRigidbody.mass, 2) * 0.5f);
			direction = Vector3.ClampMagnitude(direction, _maxForce);
			bool canLift = pullableRigidbody.mass < _maxMass;

			targetRigidbody.linearVelocity = canLift
				? direction
				: new Vector3(direction.x, pullableRigidbody.linearVelocity.y, direction.z);

			Debug.DrawLine(targetRigidbody.transform.position, _pullAnchor.position);
		}

		private void HandlePullableAdded(Unit unit)
		{
			Rigidbody pullableRigidbody = unit.GetAbility<RigidbodyAbility>();
			pullableRigidbody.linearVelocity = Vector3.zero;
			pullableRigidbody.angularVelocity = Vector3.zero;

			if(_useJoint)
			{
				_pullAnchor.position = unit.GetAbility<GrabbedAbility>().GrabPoint;
				_grabAnchor.rigidbody.transform.position = _pullAnchor.position;
				_grabAnchor.configurableJoint.connectedBody = pullableRigidbody;
			}
			else
			{
				_pullAnchor.position = pullableRigidbody.transform.position;
			}
		}

		private void HandlePullableRemoved(Unit unit)
		{
			Vector3 velocity = unit.GetAbility<RigidbodyAbility>().Rigidbody.linearVelocity;
			velocity = Vector3.ClampMagnitude(velocity, _maxThrowForce);
			unit.GetAbility<RigidbodyAbility>().Rigidbody.linearVelocity = velocity;

			if(_useJoint)
			{
				_grabAnchor.configurableJoint.connectedBody = null;
			}
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