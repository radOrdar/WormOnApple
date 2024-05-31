using System;
using Cysharp.Threading.Tasks;
using Loading;
using Services;
using Services.Ads;
using Services.Audio;
using Services.Event;
using Services.Factory;
using Services.Persistent;
using Services.ScreenLoading;
using Services.StaticData;
using StaticData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game
{
    public class GameplayRoot : MonoBehaviour
    {
        [SerializeField] private Snake snake;
        [SerializeField] private PickupSpawner pickupSpawner;
        [SerializeField] private PickupCounter pickupCounter;

        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private TextMeshProUGUI gameOverLabel;
        [SerializeField] private TextMeshProUGUI youWinLabel;
        [SerializeField] private TextMeshProUGUI levelLabel;

        [SerializeField] private Material skyMaterial;

        private IEventService _eventService;
        private IAudioService _audioService;
        private IPersistentDataService _dataService;
        private IAdsService _adsService;

        private async void Start()
        {
            ServiceLocator serviceLocator = ServiceLocator.Instance;

            nextLevelButton.onClick.AddListener(LoadGame);
            restartButton.onClick.AddListener(LoadGame);
            
            Color colorA = Color.HSVToRGB(Random.Range(0f, 1f), 0.6f, 0.9f);
            Color colorB = Color.HSVToRGB(Random.Range(0f, 1f), 0.6f, 0.9f);
            skyMaterial.SetColor("_ColorA", colorA);
            skyMaterial.SetColor("_ColorB", colorB);

            _audioService = serviceLocator.Get<IAudioService>();
            _audioService.PlayMusic();
            _dataService = serviceLocator.Get<IPersistentDataService>();
            _adsService = serviceLocator.Get<IAdsService>();
            IAdsService adsService = serviceLocator.Get<IAdsService>();
            IAPManager iapManager = serviceLocator.Get<IAPManager>();
            if (!iapManager.Subscribed && (!_dataService.TryGetSubscriptionExpirationDate(out DateTime expirationDateTime) || expirationDateTime.CompareTo(DateTime.Now) < 0))
            {
                adsService.Initialize();
                adsService.ShowBanner();
            }

            _eventService = serviceLocator.Get<IEventService>();
            _eventService.PickupsExhausted += LevelCompleted;
            _eventService.PoisonPicked += GameOver;

            LevelProgressionData levelProgressionData = await serviceLocator.Get<IStaticDataService>().GetData<LevelProgressionData>();
            int level = _dataService.GetLevel();
            levelLabel.SetText($"Level {level+1}");
            ProgressionUnit progressionUnit = levelProgressionData.GetProgression(level);
            pickupSpawner.Init(ref progressionUnit);
            snake.Init(ref progressionUnit);
            pickupCounter.Init(progressionUnit.pickupNum);
        }

        private void LoadGame()
        {
            ServiceLocator.Instance.Get<ILoadingScreenProvider>().LoadAndDestroy(new GameLoadingOperation());
        }
        
        private async void GameOver()
        {
            _audioService.PlayLost();
            gameOverLabel.gameObject.SetActive(true);
            await UniTask.Delay(3000);
            _adsService.ShowInterstitial();
            await UniTask.Delay(2000);
            restartButton.gameObject.SetActive(true);
        }

        private async void LevelCompleted()
        {
            snake.Stop();
            _audioService.PlayFinish();
            youWinLabel.gameObject.SetActive(true);
            _dataService.SaveLevel(_dataService.GetLevel() + 1);
            await UniTask.Delay(3000);
            _adsService.ShowInterstitial();
            await UniTask.Delay(2000);
            nextLevelButton.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            _eventService.PickupsExhausted -= LevelCompleted;
            _eventService.PoisonPicked -= GameOver;
        }
    }
}