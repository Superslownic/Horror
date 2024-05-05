using Scripts;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
	public class UidDrawer : OdinValueDrawer<Uid>
	{
		private GUIContent _activeLabel;

		protected override void DrawPropertyLayout(GUIContent label)
		{
			Event e = Event.current;
			Rect rect = EditorGUILayout.GetControlRect();

			if (e.type == EventType.MouseDown && e.button == 1 && rect.Contains(e.mousePosition))
			{
				GenericMenu context = new GenericMenu();
				context.AddItem(new GUIContent("Generate"), false, Generate);
				context.ShowAsContext();
			}

			if (string.IsNullOrEmpty(ValueEntry.SmartValue.Value))
			{
				Generate();
			}

			ValueEntry.SmartValue = new Uid { Value = SirenixEditorFields.TextField(rect, label, ValueEntry.SmartValue.Value) };
		}

		private void Generate()
		{
			Uid uid = new Uid();
			uid.Generate();
			ValueEntry.SmartValue = uid;
		}
	}
}