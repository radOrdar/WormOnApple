using Cysharp.Threading.Tasks;
using Loading;
using UnityEngine;

namespace Services.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assetProvider;
        
        public GameFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async UniTask WarmupAsync()
        {
           
        }

        // public UniTask<UiPopup> GetUiPopupAsync()
        // {
        //     throw new System.NotImplementedException();
        // }

        public async UniTask<LoadingScreen> GetLoadingScreenAsync()
        {
            return await _assetProvider.InstantiateAsync<LoadingScreen>(Constants.Assets.LOADING_SCREEN);
        }

        public void ReleaseInstance<T>(T instance) where T : Component
        {
            _assetProvider.ReleaseInstance(instance.gameObject);
        }

        public async UniTask<UiPopup> GetUiPopupAsync()
        {
            return await _assetProvider.InstantiateAsync<UiPopup>(Constants.Assets.UI_POPUP);
        }
    }
}