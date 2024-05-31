using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "AppodealSettings", menuName = "StaticData/AppodealSettings")]
    public class AppodealSettingsData : ScriptableObject
    {
        public bool isAppodealTestMode;
        public string appodealAppKey;
    }
}