using UnityEngine;

namespace Services.Persistent
{
    public class PersistentDataService : IPersistentDataService
    {
        public void ResetProgress()
        {
            PlayerPrefs.SetInt(Constants.PrefsKeys.LEVEL, 0);
            PlayerPrefs.Save();
        }

        public int GetLevel() => 
            PlayerPrefs.GetInt(Constants.PrefsKeys.LEVEL);

        public void SaveLevel(int level)
        {
            PlayerPrefs.SetInt(Constants.PrefsKeys.LEVEL, level);
            PlayerPrefs.Save();
        }
    }
}