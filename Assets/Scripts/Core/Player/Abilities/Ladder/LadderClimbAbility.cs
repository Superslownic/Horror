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
		private LadderValuesConfig _values;
		private float _smoothSpeed;
		private Vector3 _targetPosition;
		private float _dismountTime;
		private float _dismountTimer;
		private bool _canDismount;

		protected override void OnInitialize()
		{
			_playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();
			_triggerLink.OnEnter.AddListener(HandleTriggerEnter).AddTo(Disposable);
		}

		private void HandleTriggerEnter(Unit unit)
		{
			if(IsClimbing)
				return;

			if (unit.TryGetAbility(out LadderMarkerAbility ladder))
				Ladder = ladder;
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
					
					PlayerHeadAbility playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();
					playerHeadAbility.HeadDetachedAnchor.position =
						Vector3.Lerp(playerHeadAbility.HeadDetachedAnchor.position, playerHeadAbility.HeadStaticAnchor.position, _gameConfig.Player.Ladder.DismountCurve.Evaluate(_dismountTimer / _dismountTime));
				}
				else
				{
					IsDismounting = false;
					Unit.GetAbility<AttachHeadAbility>().RemoveDeactivator(this);
				}
			}
		}

		public void Mount()
		{
			_values = _gameConfig.Player.Ladder.DefaultValues;

			PlayerHeadAbility playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();

			Unit.GetAbility<GroundMoveAbility>().AddDeactivator(this);
			Unit.GetAbility<LookAbility>().AddDeactivator(this);
			Unit.GetAbility<AttachHeadAbility>().AddDeactivator(this);
			Unit.GetAbility<HeadBobAbility>().ReplaceConfig(_values.ShakeConfig);
			Unit.GetAbility<LeanAbility>().AddDeactivator(this);
			Unit.GetAbility<ChangeVelocityAbility>().SetActualVelocity(Vector3.zero);

			_smoothSpeed = 0;
			IsClimbing = true;
			IsDismounting = false;
			_canDismount = false;
			_targetPosition = Vector3Extensions.ClosestPointOnLine(playerHeadAbility.HeadDetachedAnchor.position, Ladder.BottomMountPoint.position, Ladder.TopMountPoint.position);

			DOTween.To(() => _smoothSpeed, value => _smoothSpeed = value, _values.MaxSmoothDelta, _values.SmoothDeltaChangeTime).SetEase(Ease.InFlash);

			float distance = Vector3.Distance(_targetPosition, playerHeadAbility.HeadDetachedAnchor.position);

			DOTween.Sequence()
				.Append(playerHeadAbility.HeadDetachedAnchor.DOMoveX(_targetPosition.x, distance * _values.MountTimeMultiplier).SetEase(Ease.InOutFlash))
				.Join(playerHeadAbility.HeadDetachedAnchor.DOMoveZ(_targetPosition.z, distance * _values.MountTimeMultiplier).SetEase(Ease.InOutFlash))
				.Join(playerHeadAbility.HeadDetachedAnchor.DORotateQuaternion(Quaternion.LookRotation(Ladder.RotationTransform.forward), distance * _values.MountTimeMultiplier).SetEase(Ease.InOutCubic))
				.AppendCallback(() =>
				{
					Unit.GetAbility<LookAbility>().RemoveDeactivator(this);
					_canDismount = true;
				});

			MountAction.Invoke();
		}

		private void Dismount(Vector3 targetPosition)
		{
			PlayerHeadAbility playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();
			PlayerBodyAbility playerBodyAbility = Unit.GetAbility<PlayerBodyAbility>();
			RigidbodyAbility rigidbodyAbility = Unit.GetAbility<RigidbodyAbility>();

			playerBodyAbility.Collider.enabled = false;
			rigidbodyAbility.Rigidbody.gameObject.SetActive(false);
			rigidbodyAbility.Rigidbody.transform.position = targetPosition;
			Unit.GetAbility<PlacePlayerOnSurfaceAbility>().Place(targetPosition);

			IsDismounting = true;
			IsClimbing = false;

			float distance = Vector3.Distance(playerHeadAbility.HeadStaticAnchor.position, playerHeadAbility.HeadDetachedAnchor.position);

			_dismountTimer = 0;
			_dismountTime = distance * _values.DismountTimeMultiplier;

			rigidbodyAbility.Rigidbody.gameObject.SetActive(true);
			playerBodyAbility.Collider.enabled = true;

			Unit.GetAbility<GroundMoveAbility>().RemoveDeactivator(this);
			Unit.GetAbility<LeanAbility>().RemoveDeactivator(this);

			Ladder = null;
			DismountAction.Invoke();
		}
	}
}