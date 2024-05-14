using DG.Tweening;
using Scripts.Configs;
using Scripts.Configs.Player;
using Scripts.Input;
using Scripts.Reactive;
using Scripts.Units;
using Scripts.Utility.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class LadderClimbAbility : Ability
	{
		public DisposableAction MountAction { get; } = new();
		public DisposableAction DismountAction { get; } = new();

		[ShowInInspector] public bool IsClimbing { get; private set; }
		[ShowInInspector] public bool IsDismounting { get; private set; }
		[ShowInInspector] public LadderMarkerAbility Ladder { get; private set; }

		[SerializeField] private TriggerLink _triggerLink;

		[Inject] private readonly InputManager _inputManager;
		[Inject] private readonly GameConfig _gameConfig;

		private PlayerHeadAbility _playerHeadAbility;
		private PlayerBodyAbility _playerBodyAbility;
		private RigidbodyAbility _rigidbodyAbility;
		private AttachHeadAbility _attachHeadAbility;
		private GroundMoveAbility _groundMoveAbility;
		private LookAbility _lookAbility;
		private HeadBobAbility _headBobAbility;
		private LeanAbility _leanAbility;
		private ChangeVelocityAbility _changeVelocityAbility;
		private PlacePlayerOnSurfaceAbility _placePlayerOnSurfaceAbility;
		private LadderValuesConfig _values;
		private float _smoothSpeed;
		private Vector3 _targetPosition;
		private float _dismountTime;
		private float _dismountTimer;
		private bool _canDismount;

		protected override void OnInitialize()
		{
			_playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();
			_playerBodyAbility = Unit.GetAbility<PlayerBodyAbility>();
			_rigidbodyAbility = Unit.GetAbility<RigidbodyAbility>();
			_attachHeadAbility = Unit.GetAbility<AttachHeadAbility>();
			_groundMoveAbility = Unit.GetAbility<GroundMoveAbility>();
			_lookAbility = Unit.GetAbility<LookAbility>();
			_headBobAbility = Unit.GetAbility<HeadBobAbility>();
			_leanAbility = Unit.GetAbility<LeanAbility>();
			_changeVelocityAbility = Unit.GetAbility<ChangeVelocityAbility>();
			_placePlayerOnSurfaceAbility = Unit.GetAbility<PlacePlayerOnSurfaceAbility>();

			_triggerLink.OnEnter.AddListener(HandleTriggerEnter).AddTo(Disposable);
			_triggerLink.OnExit.AddListener(HandleTriggerExit).AddTo(Disposable);
		}

		private void HandleTriggerEnter(Unit unit)
		{
			if(IsClimbing)
				return;

			if (unit.TryGetAbility(out LadderMarkerAbility ladder))
				Ladder = ladder;
		}

		private void HandleTriggerExit(Unit unit)
		{
			if(IsClimbing)
				return;

			if (unit.TryGetAbility(out LadderMarkerAbility ladder) && Ladder == ladder)
				Ladder = null;
		}

		protected override void OnUpdate()
		{
			if (IsClimbing)
			{
				float verticalInput = _inputManager.Move.ReadValue<Vector2>().y;
				Vector3 direction = Ladder.TopMountPoint.position - Ladder.BottomMountPoint.position;
				_targetPosition += direction.normalized * (verticalInput * _values.ClimbSpeed * Time.deltaTime);
				_playerHeadAbility.HeadDetachedAnchor.LerpPosition(_targetPosition, _smoothSpeed * Time.deltaTime);

				if(_canDismount)
				{
					if (Vector3.Distance(Ladder.BottomMountPoint.position, _targetPosition) >
					    Vector3.Distance(Ladder.BottomMountPoint.position, Ladder.TopMountPoint.position))
					{
						Dismount(Ladder.TopDismountPoint.position);
					}
					else if (Vector3.Distance(Ladder.TopMountPoint.position, _targetPosition) >
					    Vector3.Distance(Ladder.BottomMountPoint.position.AddY(_gameConfig.Player.ChangeHeight.StandConfig.HeadHeight), Ladder.TopMountPoint.position))
					{
						Dismount(Ladder.BottomDismountPoint.position);
					}
				}
				else
				{
					if (Vector3.Distance(Ladder.BottomMountPoint.position, _targetPosition) >
					    Vector3.Distance(Ladder.BottomMountPoint.position, Ladder.TopMountPoint.position))
					{
						_targetPosition = Vector3Extensions.ClosestPointOnLine(_playerHeadAbility.HeadDetachedAnchor.position, Ladder.BottomMountPoint.position, Ladder.TopMountPoint.position);
					}
					else if (Vector3.Distance(Ladder.TopMountPoint.position, _targetPosition) >
					         Vector3.Distance(Ladder.BottomMountPoint.position.AddY(_gameConfig.Player.ChangeHeight.StandConfig.HeadHeight), Ladder.TopMountPoint.position))
					{
						_targetPosition = Vector3Extensions.ClosestPointOnLine(_playerHeadAbility.HeadDetachedAnchor.position, Ladder.BottomMountPoint.position, Ladder.TopMountPoint.position);
					}
				}
			}

			if (IsDismounting)
			{
				if (_dismountTimer < _dismountTime)
				{
					_dismountTimer += Time.deltaTime;
					
					_playerHeadAbility.HeadDetachedAnchor.position =
						Vector3.Lerp(_playerHeadAbility.HeadDetachedAnchor.position, _playerHeadAbility.HeadStaticAnchor.position, _gameConfig.Player.Ladder.DismountCurve.Evaluate(_dismountTimer / _dismountTime));
				}
				else
				{
					IsDismounting = false;
					_attachHeadAbility.RemoveDeactivator(this);
				}
			}
		}

		public void Mount()
		{
			_values = _gameConfig.Player.Ladder.DefaultValues;

			_groundMoveAbility.AddDeactivator(this);
			_lookAbility.AddDeactivator(this);
			_attachHeadAbility.AddDeactivator(this);
			_headBobAbility.ReplaceConfig(_values.ShakeConfig);
			_leanAbility.AddDeactivator(this);
			_changeVelocityAbility.SetActualVelocity(Vector3.zero);

			_smoothSpeed = 0;
			IsClimbing = true;
			IsDismounting = false;
			_canDismount = false;
			_targetPosition = Vector3Extensions.ClosestPointOnLine(_playerHeadAbility.HeadDetachedAnchor.position, Ladder.BottomMountPoint.position, Ladder.TopMountPoint.position);

			DOTween.To(() => _smoothSpeed, value => _smoothSpeed = value, _values.MaxSmoothDelta, _values.SmoothDeltaChangeTime).SetEase(Ease.InFlash);

			float distance = Vector3.Distance(_targetPosition, _playerHeadAbility.HeadDetachedAnchor.position);

			DOTween.Sequence()
				.Append(_playerHeadAbility.HeadDetachedAnchor.DOMoveX(_targetPosition.x, distance * _values.MountTimeMultiplier).SetEase(Ease.InOutFlash))
				.Join(_playerHeadAbility.HeadDetachedAnchor.DOMoveZ(_targetPosition.z, distance * _values.MountTimeMultiplier).SetEase(Ease.InOutFlash))
				.Join(_playerHeadAbility.HeadDetachedAnchor.DORotateQuaternion(Quaternion.LookRotation(Ladder.RotationTransform.forward), distance * _values.MountTimeMultiplier).SetEase(Ease.InOutCubic))
				.AppendCallback(() =>
				{
					_lookAbility.RemoveDeactivator(this);
					_canDismount = true;
				});

			MountAction.Invoke();
		}

		private void Dismount(Vector3 targetPosition)
		{
			_playerBodyAbility.Collider.enabled = false;
			_rigidbodyAbility.Rigidbody.gameObject.SetActive(false);
			_rigidbodyAbility.Rigidbody.transform.position = targetPosition;
			_placePlayerOnSurfaceAbility.Place(targetPosition);

			IsDismounting = true;
			IsClimbing = false;

			float distance = Vector3.Distance(_playerHeadAbility.HeadStaticAnchor.position, _playerHeadAbility.HeadDetachedAnchor.position);

			_dismountTimer = 0;
			_dismountTime = distance * _values.DismountTimeMultiplier;

			_rigidbodyAbility.Rigidbody.gameObject.SetActive(true);
			_playerBodyAbility.Collider.enabled = true;
			_groundMoveAbility.RemoveDeactivator(this);
			_leanAbility.RemoveDeactivator(this);

			Ladder = null;
			DismountAction.Invoke();
		}
	}
}