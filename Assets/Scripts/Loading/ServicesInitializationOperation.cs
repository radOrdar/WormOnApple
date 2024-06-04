using System;
using Cysharp.Threading.Tasks;
using Services;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

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
                Debug.Log("OPERATION INIT SERVICES IS ABOUT TO START");
                var options = new InitializationOptions()
                    .SetEnvironmentName("production");

                await UnityServices.InitializeAsync(options);
                _gamingServicesInitialized = true;
                Debug.Log("OPERATION INIT SERVICES COMPLETED");
            } catch (Exception exception)
            {
                Debug.Log("OPERATION INIT SERVICES ERROR");
            }

            onProgress(0.5f);
            await UniTask.Delay(500);

            if (_gamingServicesInitialized)
            {
                Debug.Log("OPERATION INIT IAP IS ABOUT TO START");
                IAPManager iapManager = ServiceLocator.Instance.Get<IAPManager>();
                iapManager.InitializationFinished += () => _iapInitializationFinished = true;
                iapManager.Init();
                float timeOut = 0;
                while (_iapInitializationFinished == false)
                {
                    timeOut += Time.deltaTime;
                    if (timeOut > 3)
                    {
                        break;
                    }
                    await UniTask.Yield();
                }

                Debug.Log("OPERATION INIT SERVICES COMPLETE");
            }
            
        }
    }
}