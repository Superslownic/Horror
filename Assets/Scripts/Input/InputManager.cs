using UnityEngine;

namespace Scripts.Input
{
	public class InputManager
	{
		private InputActions _inputActions = new();

		public void Initialize()
		{
			_inputActions.Enable();
		}

		public Vector2 GetMoveValue()
		{
			return _inputActions.Default.Move.ReadValue<Vector2>();
		}

		public Vector2 GetRotationValue()
		{
			return _inputActions.Default.Rotation.ReadValue<Vector2>();
		}
	}
}