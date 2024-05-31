using StaticData;
using UnityEngine;

namespace Services.Audio
{
    public class AudioService : IAudioService
    {
        private SoundsData _soundsData;
        private AudioSource _audioSource;
        
        public AudioService(SoundsData soundsData)
        {
            _soundsData = soundsData;
            _audioSource = new GameObject("AudioSource").AddComponent<AudioSource>();
            _audioSource.loop = true;
            Object.DontDestroyOnLoad(_audioSource);
        }

        public void PlayFinish()
        {
            _audioSource.PlayOneShot(_soundsData.finish);
        }

        public void PlayLost()
        {
           _audioSource.PlayOneShot(_soundsData.lost);
        }

        public void PlayPickup()
        {
            _audioSource.PlayOneShot(_soundsData.pickup);
        }

        public void PlayMusic()
        {
            _audioSource.clip = _soundsData.music;
            _audioSource.Play();
        }
    }
}
