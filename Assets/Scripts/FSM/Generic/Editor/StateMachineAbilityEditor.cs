using System.Text;
using Scripts.FSM.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Generic.Editor
{
	[CustomEditor(typeof(StateMachineAbility), true)]
	public class StateMachineAbilityEditor : UnityEditor.Editor
	{
		private StateMachineAbility _stateMachineAbility;
		
		private void OnEnable()
		{
			_stateMachineAbility = target as StateMachineAbility;
		}

		public override VisualElement CreateInspectorGUI()
		{
			StringBuilder stringBuilder = new();
			
			if (Application.isPlaying)
			{
				GetPath(_stateMachineAbility.StateMachine.CurrentState, stringBuilder);
			}
			
			return new Label(stringBuilder.ToString());
		}

		private void GetPath(State state, StringBuilder stringBuilder)
		{
			if (state == null)
			{
				return;
			}

			stringBuilder.Append($"<b>{state.Name}</b>");
			
			if (state is StateMachine stateMachine)
			{
				stringBuilder.Append(" ➜ ");
				GetPath(stateMachine.CurrentState, stringBuilder);
			}
		}
	}
}