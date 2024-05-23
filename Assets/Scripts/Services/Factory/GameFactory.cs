using Cysharp.Threading.Tasks;
using Loading;
using Services.Asset;
using UnityEngine;

namespace Services.Factory
{
    public class GameFactory : IGameFactory
    {
        public GameFactory(AssetProvider assetProvider)
        {
            throw new System.NotImplementedException();
        }

        public UniTask WarmupAsync()
        {
            throw new System.NotImplementedException();
        }

        // public UniTask<UiPopup> GetUiPopupAsync()
        // {
        //     throw new System.NotImplementedException();
        // }

        public UniTask<LoadingScreen> GetLoadingScreenAsync()
        {
            throw new System.NotImplementedException();
        }

        public void ReleaseInstance<T>(T instance) where T : Component
        {
            throw new System.NotImplementedException();
        }
    }
}