namespace Services.Ads
{
    public interface IAdsService : IService
    {
        void Initialize();
        void ShowInterstitial();
        void ShowBanner();
        void HideBanner();
    }
}