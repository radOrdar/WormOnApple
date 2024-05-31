using System;

namespace Services.Event
{
    public class EventService : IEventService
    {
        public event Action AdsRemoved;
        public event Action PickupPicked;
        public event Action PoisonPicked;
        public event Action PickupsExhausted;

        public void OnAdsRemoved()
        {
            AdsRemoved?.Invoke();
        }

        public void OnPickupPicked()
        {
            PickupPicked?.Invoke();
        }

        public void OnPickupsExhausted()
        {
            PickupsExhausted?.Invoke();
        }

        public void OnPoisonPicked()
        {
            PoisonPicked?.Invoke();
        }
    }
}