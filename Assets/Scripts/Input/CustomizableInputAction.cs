using UnityEngine.InputSystem;

namespace Scripts.Input
{
	public abstract class CustomizableInputAction
	{
		protected InputAction InputAction { get; }

		protected CustomizableInputAction(InputAction inputAction)
		{
			InputAction = inputAction;
		}

		public object ReadValueAsObject()
		{
			return InputAction.ReadValueAsObject();
		}

		public void Reset()
		{
			InputAction.Reset();
		}

		public bool IsPressed()
		{
			return InputAction.IsPressed();
		}

		public bool IsInProgress()
		{
			return InputAction.IsInProgress();
		}

		public bool WasPressedThisFrame()
		{
			return InputAction.WasPressedThisFrame();
		}

		public bool WasReleasedThisFrame()
		{
			return InputAction.WasReleasedThisFrame();
		}

		public float GetTimeoutCompletionPercentage()
		{
			return InputAction.GetTimeoutCompletionPercentage();
		}

		public bool WasPerformedThisFrame()
		{
			return InputAction.WasPerformedThisFrame();
		}

		public T ReadValue<T>() where T : struct
		{
			return InputAction.ReadValue<T>();
		}
	}
}