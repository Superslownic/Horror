using UnityEngine.InputSystem;

namespace Scripts.Input
{
	public class ToggleInputAction : CustomizableInputAction
	{
		public bool IsToggled { get; private set; }

		public ToggleInputAction(InputAction inputAction) : base(inputAction)
		{
			InputAction.performed += HandleInputActionPerformed;
		}

		private void HandleInputActionPerformed(InputAction.CallbackContext callbackContext)
		{
			IsToggled = !IsToggled;
		}
	}
}