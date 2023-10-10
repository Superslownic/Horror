using System.Text;
using Scripts.FSM.Composite;
using Scripts.Reactive;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FSM.Composite.Editor
{
	[CustomEditor(typeof(StateMachineAbility), true)]
	public class StateMachineAbilityEditor : UnityEditor.Editor
	{
		private StateMachineAbility _stateMachineAbility;
		private StringBuilder _stringBuilder;
		private Label _label;
		private CompositeDisposable _disposable;
		
		private void OnEnable()
		{
			_stateMachineAbility = target as StateMachineAbility;
			_stringBuilder = new StringBuilder();
			_label = new Label();
			_disposable = new CompositeDisposable();
		}

		public override VisualElement CreateInspectorGUI()
		{
			if (!Application.isPlaying)
			{
				_label.text = "Information will appear in play mode";
			}
			else
			{
				_stateMachineAbility
					.Root
					.StateChanged
					.AddListener(HandleStateChanged)
					.WithInvoke()
					.AddTo(_disposable);
			}

			return _label;
		}

		private void OnDisable()
		{
			_disposable.Dispose();
		}

		private void HandleStateChanged()
		{
			_stringBuilder.Clear();
			CalculateCurrentStateLabel(_stateMachineAbility.Root.CurrentState, _stringBuilder);
			_label.text = _stringBuilder.ToString();
		}

		private void CalculateCurrentStateLabel(State state, StringBuilder stringBuilder)
		{
			if (state == null)
			{
				return;
			}

			stringBuilder.Append($"<b>{state.Name}</b>");
			
			if (state is SuperState stateMachine)
			{
				stringBuilder.Append("/");
				CalculateCurrentStateLabel(stateMachine.CurrentState, stringBuilder);
			}
		}
	}
}