using System;
using Cysharp.Threading.Tasks;
using Services.Event;
using Services.Factory;
using Services.Persistent;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Services
{
    public class IAPManager : IDetailedStoreListener, IService
    {
        private const string SUB_ID = "com.onimka.worm.stopads";

        private static IStoreController _storeController;
        private static IExtensionProvider _extensionProvider;


        public Action InitializationFinished;
        public bool Subscribed { get; private set; }

        public void InitiatePurchase()
        {
            Debug.Log("Initiate Purchasing");
            _storeController.InitiatePurchase(SUB_ID);
        }
        
        private bool CheckSubscribe()
        {
            Product product = _storeController.products.WithID(SUB_ID);

            if (product != null && product.hasReceipt)
            {
                var subManager = new SubscriptionManager(product, null);
                var info = subManager.getSubscriptionInfo();
                if (info.isSubscribed() == Result.True)
                {
                    Subscribed = true;
                    ServiceLocator.Instance.Get<IEventService>().OnAdsRemoved();
                    ServiceLocator.Instance.Get<IPersistentDataService>().SaveSubscriptionExpirationDate(info.getExpireDate());
                    return true;
                } 
            }

            return false;
        }


        public void Init()
        {
            if (_storeController == null)
                InitializePurchasing();
        }

        private void InitializePurchasing()
        {
            if (IsIAPInitialized())
            {
                Debug.Log("IAP Initialized");
                return;
            }

            Debug.Log("IAP is Initialize...");
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            builder.AddProduct(SUB_ID, ProductType.Subscription);
            builder.Configure<IAmazonConfiguration>();
            UnityPurchasing.Initialize(this, builder);
        }


        public void OnInitializeFailed(InitializationFailureReason error, string message = "")
        {
            Debug.Log($"OnInitializeFailed InitializationFailureReason: {error}");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failReason)
        {
            if (failReason == PurchaseFailureReason.PurchasingUnavailable)
            {
                Debug.Log($"OnPurchaseFailed: FAIL. Product: '{product.definition.storeSpecificId}', PurchaseFailureReason: {failReason}");
            }
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            var product = purchaseEvent.purchasedProduct;
            if (product.definition.id == SUB_ID)
                if (CheckSubscribe())
                {
                    ShowPopup(true);
                }


            return PurchaseProcessingResult.Complete;
        }

        private async UniTask ShowPopup(bool success)
        {
            IGameFactory gameFactory = ServiceLocator.Instance.Get<IGameFactory>();
            UiPopup uiPopup = await gameFactory.GetUiPopupAsync();
            // UiPopup uiPopup = await _gameFactory.InstantiateAsync<UiPopup>(Constants.Assets.UI_POPUP);
            await uiPopup.AwaitForCompletion(success? "Ads Removed!" : "Smth went wrong Try again later");
            gameFactory.ReleaseInstance(uiPopup);
        }
        
        private bool IsIAPInitialized()
        {
            return _storeController != null && _extensionProvider != null;
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _storeController = controller;
            _extensionProvider = extensions;

            CheckSubscribe();
            
            InitializationFinished?.Invoke();
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
         
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.LogError("IAP initialization failed: " + error);
            InitializationFinished?.Invoke();
        }
    }
}