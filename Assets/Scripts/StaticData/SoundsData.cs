using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "StaticData/SoundsData")]
    public class SoundsData : ScriptableObject
    {
        public AudioClip finish;
        public AudioClip music;
        public AudioClip lost;
        public AudioClip pickup;
    }
}