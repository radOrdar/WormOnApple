using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Loading;
using Services;
using Services.Asset;
using Services.Audio;
using Services.Factory;
using Services.Input;
using Services.ScreenLoading;
using Services.StaticData;
using StaticData;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class AppStartup : MonoBehaviour
    {
        private async void Awake()
        {
            DontDestroyOnLoad(this);
            if (SceneManager.GetActiveScene().name != Constants.Scenes.STARTUP)
            {
                await SceneManager.LoadSceneAsync(Constants.Scenes.STARTUP);
            }

            await RegisterServices();
            
            var loadingOperations = new Queue<ILoadingOperation>();
            loadingOperations.Enqueue(new ConfigLoadingOperation());
            loadingOperations.Enqueue(new AssetInitializeOperation());
            loadingOperations.Enqueue(new MenuLoadingOperation());
            
            
        }

        private async UniTask RegisterServices()
        {
            ServiceLocator serviceLocator = ServiceLocator.Instance;
            
            AssetProvider assetProvider = new AssetProvider();
            StaticDataService staticDataService = new StaticDataService(assetProvider);
            GameFactory gameFactory = new GameFactory(assetProvider);

            // serviceLocator.Register<IEventService>(new EventService());
            // serviceLocator.Register<IPersistentDataService>(new PersistentDataService());
            serviceLocator.Register<IAssetProvider>(assetProvider);

            Joystick joystick = await assetProvider.InstantiateAsync<Joystick>(Constants.Assets.JOYSTICK);
            serviceLocator.Register<IInputService>(new InputService(joystick));
            serviceLocator.Register<IStaticDataService>(staticDataService);
            serviceLocator.Register<IGameFactory>(gameFactory);
            serviceLocator.Register<ILoadingScreenProvider>(new LoadingScreenProvider(gameFactory));
            
            SoundsData soundsData = await staticDataService.GetData<SoundsData>();
            serviceLocator.Register<IAudioService>(new AudioService(soundsData));
            
            AppConfigurationData appConfigurationData = await staticDataService.GetData<AppConfigurationData>();
            // serviceLocator.Register<IAdsService>(new AdsService(appConfigurationData));

        }

        private void OnDestroy()
        {
            ServiceLocator.Instance.Dispose();
        }
    }
}