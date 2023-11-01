using Scripts.Entities;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class PlayerBodyAbility : Ability
	{
		[field: SerializeField] public CharacterController CharacterController { get; private set; }

		public float Height => CharacterController.height + CharacterController.radius * 2;
	}
}