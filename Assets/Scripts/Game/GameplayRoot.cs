using Services;
using Services.Persistent;
using Services.StaticData;
using StaticData;
using UnityEngine;

namespace Game
{
    public class GameplayRoot : MonoBehaviour
    {
        [SerializeField] private Snake snake;
        [SerializeField] private PickupSpawner pickupSpawner;

        private async void Start()
        {
            ServiceLocator serviceLocator = ServiceLocator.Instance;
            IPersistentDataService persistentDataService = serviceLocator.Get<IPersistentDataService>();
            LevelProgressionData levelProgressionData = await serviceLocator.Get<IStaticDataService>().GetData<LevelProgressionData>();

            ProgressionUnit progressionUnit = levelProgressionData.GetProgression(persistentDataService.GetLevel());
            pickupSpawner.Init(ref progressionUnit);
            snake.Init(ref progressionUnit);
        }
    }
}