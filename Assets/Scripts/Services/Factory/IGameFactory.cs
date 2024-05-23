using Cysharp.Threading.Tasks;
using Loading;
using UnityEngine;

namespace Services.Factory
{
    public interface IGameFactory : IService
    {
        UniTask WarmupAsync();
        // UniTask<UiPopup> GetUiPopupAsync();
        UniTask<LoadingScreen> GetLoadingScreenAsync();
        void ReleaseInstance<T>(T instance) where T : Component;
    }
}