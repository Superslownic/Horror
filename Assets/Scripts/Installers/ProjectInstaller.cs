using Scripts.Config;
using Scripts.Factory;
using Scripts.FSM;
using Scripts.Input;
using UnityEngine;
using Zenject;

namespace Scripts.Installers
{
	public class ProjectInstaller : MonoInstaller
	{
		[SerializeField] private GameConfig _gameConfig;
		
		public override void InstallBindings()
		{
			Container.BindInstance(_gameConfig);
			Container.BindInterfacesAndSelfTo<ObjectFactory>();
			Container.BindInterfacesAndSelfTo<InputManager>();
			Container.BindInterfacesAndSelfTo<GameStateMachine>().NonLazy();
		}
	}
}