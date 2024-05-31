using System;
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

        public bool TryGetSubscriptionExpirationDate(out DateTime dateTime)
        {
            string s = PlayerPrefs.GetString(Constants.PrefsKeys.SUBSCRIPTION_EXPIRATION);
            if (string.IsNullOrEmpty(s) == false)
            {
                DateTimeDTO dateTimeDto = JsonUtility.FromJson<DateTimeDTO>(s);
                dateTime = dateTimeDto.ToDateTime();
                return true;
            }

            dateTime = default;
            return false;
        }

        public void SaveSubscriptionExpirationDate(DateTime dateTime)
        {
            PlayerPrefs.SetString(Constants.PrefsKeys.SUBSCRIPTION_EXPIRATION, JsonUtility.ToJson(new DateTimeDTO(dateTime.Day, dateTime.Month, dateTime.Year)));
            PlayerPrefs.Save();
        }
    }
    
    [Serializable]
    public struct DateTimeDTO
    {
        public int Day, Month, Year;

        public DateTimeDTO(int day, int month, int year)
        {
            Day = day;
            Month = month;
            Year = year;
        }

        public DateTime ToDateTime() => 
            new(year: Year, day: Day, month: Month);
    }
}