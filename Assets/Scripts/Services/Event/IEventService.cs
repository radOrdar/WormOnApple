using System;

namespace Services.Event
{
    public interface IEventService : IService
    {
        event Action AdsRemoved;
        event Action PickupPicked;
        event Action PoisonPicked;
        event Action PickupsExhausted;
        
        void OnAdsRemoved();
        void OnPickupPicked();
        void OnPickupsExhausted();
        void OnPoisonPicked();
    }
}