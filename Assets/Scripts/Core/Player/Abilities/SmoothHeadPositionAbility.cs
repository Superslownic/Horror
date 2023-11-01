using Scripts.Config;
using Scripts.Entities;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class SmoothHeadPositionAbility : Ability
	{
		[SerializeField] private Transform _anchor;
		[SerializeField] private Transform _target;
		
		[Inject] private readonly GameConfig _gameConfig;
		
		protected override void OnLateUpdate()
		{
			_anchor.position = Vector3.Lerp(_anchor.position, _target.position, _gameConfig.Player.Look.PositionInterpolationSpeed * Time.deltaTime);
		}
	}
}