using Services;
using Services.Event;
using TMPro;
using UnityEngine;

namespace Game
{
    public class PickupCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        private int _pikupsLeft;
        private IEventService _eventService;

        public void Init(int pickupsNum)
        {
            _eventService = ServiceLocator.Instance.Get<IEventService>();
            
            _pikupsLeft = pickupsNum;
            UpdatePickupText();

            _eventService.PickupPicked += PickupPicked;
        }

        private void PickupPicked()
        {
            _pikupsLeft--;
            UpdatePickupText();

            if (_pikupsLeft == 0)
            {
                _eventService.OnPickupsExhausted();
            }
        }

        private void UpdatePickupText()
        {
            scoreText.SetText($"{_pikupsLeft.ToString()} left");
        }

        private void OnDestroy()
        {
            _eventService.PickupPicked -= PickupPicked;
        }
    }
}