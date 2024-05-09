using Scripts.Audio;
using Scripts.Configs;
using Scripts.Factory;
using Scripts.Game.States;
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
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			Container.BindInstance(_gameConfig).AsSingle();
			Container.BindInterfacesAndSelfTo<ObjectFactory>().AsSingle();
			Container.BindInterfacesAndSelfTo<InputManager>().AsSingle();
			Container.BindMemoryPool<Sound, Sound.Pool>().WithInitialSize(10);
			Container.BindInterfacesAndSelfTo<AudioManager>().AsSingle();
			Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle().NonLazy();
		}
	}
}