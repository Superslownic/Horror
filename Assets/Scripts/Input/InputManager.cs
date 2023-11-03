using Scripts.Reactive;
using UnityEngine.InputSystem;

namespace Scripts.Input
{
	public class InputManager
	{
		public DisposableAction OnInputTypeChanged { get; } = new();
		public InputType CurrentInputType { get; private set; }
		public InputAction Move => _inputActions.Default.Move;
		public InputAction Look => _inputActions.Default.Look;
		public InputAction Crouch => _inputActions.Default.Crouch;
		public InputAction Run => _inputActions.Default.Run;
		public InputAction Flashlight => _inputActions.Default.Flashlight;

		private InputActions _inputActions = new();

		public void Initialize()
		{
			InputSystem.onActionChange += HandleActionChange;
			_inputActions.Enable();
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

				if (CurrentInputType != inputType)
				{
					CurrentInputType = inputType;
					OnInputTypeChanged.Invoke();
				}
			}
		}
	}
}