using Scripts.Config;
using Scripts.Entities;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Camera
{
	public class CameraSmoothAbility : Ability
	{
		[SerializeField] private Transform _anchor;
		[SerializeField] private Transform _target;

		[Inject] private readonly GameConfig _gameConfig;

		private Vector3 _position;

		protected override void OnInitialize()
		{
			_position = _anchor.position;
		}

		protected override void OnUpdate()
		{
			_position = Vector3.Lerp(_position, _target.position, _gameConfig.Camera.Smooth.Force * Time.deltaTime);
			_anchor.position = _position;
		}
	}
}