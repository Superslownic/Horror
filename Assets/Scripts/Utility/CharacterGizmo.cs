using UnityEditor;
using UnityEngine;

namespace Scripts.Utility
{
	[RequireComponent(typeof(CharacterController))]
	public class CharacterGizmo : MonoBehaviour
	{
		private CharacterController _characterController;

		private CharacterController CharacterController
		{
			get
			{
				if (_characterController == null)
				{
					_characterController = GetComponent<CharacterController>();
				}

				return _characterController;
			}
		}

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Vector3 position = transform.TransformPoint(CharacterController.center);
			float height = CharacterController.height;
			float radius = CharacterController.radius;
			
			Matrix4x4 angleMatrix = Matrix4x4.TRS(position, transform.rotation, Handles.matrix.lossyScale);
			
			using (new Handles.DrawingScope(angleMatrix))
			{
				var pointOffset = (height - radius * 2) / 2;
 
				//draw sideways
				Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, radius);
				Handles.DrawLine(new Vector3(0, pointOffset, -radius), new Vector3(0, -pointOffset, -radius));
				Handles.DrawLine(new Vector3(0, pointOffset, radius), new Vector3(0, -pointOffset, radius));
				Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, radius);
				//draw frontways
				Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, radius);
				Handles.DrawLine(new Vector3(-radius, pointOffset, 0), new Vector3(-radius, -pointOffset, 0));
				Handles.DrawLine(new Vector3(radius, pointOffset, 0), new Vector3(radius, -pointOffset, 0));
				Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, radius);
				//draw center
				Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, radius);
				Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, radius);
			}
		}
		#endif
	}
}