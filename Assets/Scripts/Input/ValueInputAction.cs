using UnityEngine.InputSystem;

namespace Scripts.Input
{
	public class ValueInputAction<T> : CustomizableInputAction where T : struct
	{
		public T Value => InputAction.ReadValue<T>();

		public ValueInputAction(InputAction inputAction) : base(inputAction)
		{
		}
	}
}