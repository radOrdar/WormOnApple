using System;

namespace Services.Persistent
{
    public interface IPersistentDataService : IService
    {
        void ResetProgress();

        int GetLevel();

        void SaveLevel(int level);
        bool TryGetSubscriptionExpirationDate(out DateTime dateTime);

        void SaveSubscriptionExpirationDate(DateTime dateTime);
    }
}