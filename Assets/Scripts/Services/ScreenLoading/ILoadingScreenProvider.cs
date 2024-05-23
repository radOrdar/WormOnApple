using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Loading;

namespace Services.ScreenLoading
{
    public interface ILoadingScreenProvider : IService
    {
        UniTask LoadAndDestroy(ILoadingOperation loadingOperation);
        UniTask LoadAndDestroy(Queue<ILoadingOperation> loadingOperations);
    }
}