using DG.Tweening;
using Scripts.Configs;
using Scripts.Configs.Player;
using Scripts.Input;
using Scripts.Reactive;
using Scripts.Units;
using Scripts.Utility.Extensions;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class LadderClimbAbility : Ability
	{
		public DisposableAction MountAction { get; } = new();
		public DisposableAction DismountAction { get; } = new();

		public LadderMarkerAbility Ladder { get; private set; }

		[SerializeField] private TriggerLink triggerLink;

		[Inject] private readonly InputManager _inputManager;
		[Inject] private readonly GameConfig _gameConfig;

		private LadderValuesConfig _values;
		private float _smoothSpeed;
		private float _height;
		private float _dismountTime;
		private float _dismountTimer;
		private bool _isClimbing;
		private bool _isDismounting;
		private bool _canDismount;

		protected override void OnInitialize()
		{
			triggerLink.OnEnter.AddListener(HandleTriggerEnter).AddTo(Disposable);
		}

		private void HandleTriggerEnter(Unit unit)
		{
			if (unit.TryGetAbility(out LadderMarkerAbility ladder))
			{
				Ladder = ladder;
				Mount();
				MountAction.Invoke();
			}
		}

		protected override void OnUpdate()
		{
			if (_isClimbing)
			{
				float input = _inputManager.Move.ReadValue<Vector2>().y;

				_height += input * _values.ClimbSpeed * Time.deltaTime;

				if (!_canDismount)
				{
					_height = Mathf.Clamp01(_height);
				}

				Vector3 targetPosition = Vector3.Lerp(Ladder.BottomMountPoint.position, Ladder.TopMountPoint.position, _height);
				PlayerHeadAbility playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();
				targetPosition.x = playerHeadAbility.HeadDetachedAnchor.position.x;
				targetPosition.z = playerHeadAbility.HeadDetachedAnchor.position.z;
				playerHeadAbility.HeadDetachedAnchor.position = Vector3.Lerp(playerHeadAbility.HeadDetachedAnchor.position, targetPosition, _smoothSpeed * Time.deltaTime);

				if (_canDismount)
				{
					if (_height < 0)
					{
						Dismount(Ladder.BottomDismountPoint.position);
					}
				
					if (_height > 1)
					{
						Dismount(Ladder.TopDismountPoint.position);
					}
				}
			}

			if (_isDismounting)
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
					_isDismounting = false;
					Unit.GetAbility<AttachHeadAbility>().RemoveDeactivator(this);
				}
			}
		}

		private void Mount()
		{
			_values = _gameConfig.Player.Ladder.DefaultValues;

			PlayerHeadAbility playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();

			Unit.GetAbility<GroundMoveAbility>().AddDeactivator(this);
			Unit.GetAbility<LookAbility>().AddDeactivator(this);
			Unit.GetAbility<AttachHeadAbility>().AddDeactivator(this);
			Unit.GetAbility<HeadBobAbility>().ReplaceConfig(_values.ShakeConfig);
			Unit.GetAbility<LeanAbility>().AddDeactivator(this);

			_smoothSpeed = 0;
			_isClimbing = true;
			_isDismounting = false;
			_canDismount = false;

			Vector3 mountPosition = Vector3Extensions.ClosestPointOnLine(playerHeadAbility.HeadDetachedAnchor.position, Ladder.BottomMountPoint.position, Ladder.TopMountPoint.position);

			_height = Mathf.InverseLerp(Ladder.BottomMountPoint.position.y, Ladder.TopMountPoint.position.y, mountPosition.y);

			DOTween.To(() => _smoothSpeed, value => _smoothSpeed = value, _values.MaxSmoothDelta, _values.SmoothDeltaChangeTime).SetEase(Ease.InFlash);
			
			float distance = Vector3.Distance(mountPosition, playerHeadAbility.HeadDetachedAnchor.position);

			DOTween.Sequence()
				.Append(playerHeadAbility.HeadDetachedAnchor.DOMoveX(mountPosition.x, distance * _values.MountTimeMultiplier).SetEase(Ease.InOutFlash))
				.Join(playerHeadAbility.HeadDetachedAnchor.DOMoveZ(mountPosition.z, distance * _values.MountTimeMultiplier).SetEase(Ease.InOutFlash))
				.Join(playerHeadAbility.HeadDetachedAnchor.DORotateQuaternion(Quaternion.LookRotation(Ladder.RotationTransform.forward), distance * _values.MountTimeMultiplier).SetEase(Ease.InOutFlash))
				.AppendCallback(() =>
				{
					Unit.GetAbility<LookAbility>().RemoveDeactivator(this);
					_canDismount = true;
				});
		}

		private void Dismount(Vector3 targetPosition)
		{
			PlayerHeadAbility playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();
			PlayerBodyAbility playerBodyAbility = Unit.GetAbility<PlayerBodyAbility>();
			RigidbodyAbility rigidbodyAbility = Unit.GetAbility<RigidbodyAbility>();

			playerBodyAbility.Collider.enabled = false;
			rigidbodyAbility.Rigidbody.gameObject.SetActive(false);
			rigidbodyAbility.Rigidbody.transform.position = targetPosition;
            
			_isDismounting = true;
			_isClimbing = false;	
					
			float distance = Vector3.Distance(playerHeadAbility.HeadStaticAnchor.position, playerHeadAbility.HeadDetachedAnchor.position);
			
			_dismountTimer = 0;
			_dismountTime = distance * _values.DismountTimeMultiplier;
			
			Unit.GetAbility<GroundMoveAbility>().RemoveDeactivator(this);
			Unit.GetAbility<LeanAbility>().RemoveDeactivator(this);

			rigidbodyAbility.Rigidbody.gameObject.SetActive(true);
			playerBodyAbility.Collider.enabled = true;
			
			DismountAction.Invoke();
		}
	}
}