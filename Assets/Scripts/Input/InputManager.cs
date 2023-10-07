using Scripts.Reactive;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Input
{
	public class InputManager
	{
		public DisposableAction OnInputTypeChanged { get; } = new();
		
		private InputActions _inputActions = new();
		private InputType _currentInputType;

		public void Initialize()
		{
			_inputActions.Enable();
			InputSystem.onActionChange += HandleActionChange;
		}

		private void HandleActionChange(object obj, InputActionChange inputActionChange)
		{
			if (inputActionChange == InputActionChange.ActionPerformed)
			{
				InputAction receivedInputAction = (InputAction) obj;
				InputDevice lastDevice = receivedInputAction.activeControl.device;

				var inputType = lastDevice switch
				{
					Keyboard or Mouse => InputType.Keyboard,
					Gamepad => InputType.Gamepad,
					_ => InputType.Keyboard
				};

				if (_currentInputType != inputType)
				{
					_currentInputType = inputType;
					OnInputTypeChanged.Invoke();
				}
			}
		}

		public Vector2 GetMoveValue()
		{
			return _inputActions.Default.Move.ReadValue<Vector2>();
		}

		public Vector2 GetRotationValue()
		{
			return _inputActions.Default.Rotation.ReadValue<Vector2>();
		}

		public InputType GetCurrentInputType()
		{
			return _currentInputType;
		}
	}
}