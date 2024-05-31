using System;
using Cysharp.Threading.Tasks;
using Services;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

namespace Loading
{
    public class ServicesInitializationOperation : ILoadingOperation
    {
        public string Description => "Initializing services...";

        private bool _gamingServicesInitialized = false;

        private bool _iapInitializationFinished = false;
        public async UniTask Load(Action<float> onProgress)
        {
            onProgress(0.1f);
            try
            {
                var options = new InitializationOptions()
                    .SetEnvironmentName("production");

                await UnityServices.InitializeAsync(options);
                _gamingServicesInitialized = true;
            } catch (Exception exception)
            {
                // An error occurred during services initialization.
            }

            onProgress(0.5f);
            await UniTask.Delay(500);

            if (_gamingServicesInitialized)
            {
                IAPManager iapManager = ServiceLocator.Instance.Get<IAPManager>();
                iapManager.InitializationFinished += () => _iapInitializationFinished = true;
                iapManager.Init();
                while (_iapInitializationFinished == false)
                {
                   await UniTask.Yield();
                }    
            }
            
        }
    }
}