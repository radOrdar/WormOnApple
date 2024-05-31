using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Loading;
using Services;
using Services.Ads;
using Services.Asset;
using Services.Audio;
using Services.Event;
using Services.Factory;
using Services.Persistent;
using Services.ScreenLoading;
using Services.StaticData;
using StaticData;
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
            loadingOperations.Enqueue(new ServicesInitializationOperation());
            loadingOperations.Enqueue(new ConfigLoadingOperation());
            // loadingOperations.Enqueue(new AssetInitializeOperation());
            loadingOperations.Enqueue(new MenuLoadingOperation());

            ServiceLocator.Instance.Get<ILoadingScreenProvider>().LoadAndDestroy(loadingOperations);
        }

        private async UniTask RegisterServices()
        {
            ServiceLocator serviceLocator = ServiceLocator.Instance;
            
            AssetProvider assetProvider = new AssetProvider();
            StaticDataService staticDataService = new StaticDataService(assetProvider);
            GameFactory gameFactory = new(assetProvider);

            serviceLocator.Register<IEventService>(new EventService());
            serviceLocator.Register<IAssetProvider>(assetProvider);

            // Joystick joystick = await assetProvider.InstantiateAsync<Joystick>(Constants.Assets.JOYSTICK);
            // serviceLocator.Register<IInputService>(new InputService(joystick));
            serviceLocator.Register<IStaticDataService>(staticDataService);
            serviceLocator.Register<IGameFactory>(gameFactory);
            serviceLocator.Register<ILoadingScreenProvider>(new LoadingScreenProvider(gameFactory));
            serviceLocator.Register<IPersistentDataService>(new PersistentDataService());
            serviceLocator.Register<IAdsService>(new AdsService(await staticDataService.GetData<AppodealSettingsData>()));
            serviceLocator.Register<IAPManager>(new IAPManager());
            
            SoundsData soundsData = await staticDataService.GetData<SoundsData>();
            serviceLocator.Register<IAudioService>(new AudioService(soundsData));

        }

        private void OnDestroy()
        {
            ServiceLocator.Instance.Dispose();
        }
    }
}