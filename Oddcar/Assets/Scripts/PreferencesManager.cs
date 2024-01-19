using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferencesManager : MonoBehaviour
{
    private const string UnlockedLevelsKey = "UnlockedLevels";

    public static void SaveUnlockedLevels(int[] unlockedLevels)
    {
        string unlockedLevelsString = ArrayToString(unlockedLevels);
        PlayerPrefs.SetString(UnlockedLevelsKey, unlockedLevelsString);
        PlayerPrefs.Save();
    }

    public static int[] GetUnlockedLevels()
    {
        string unlockedLevelsString = PlayerPrefs.GetString(UnlockedLevelsKey, "0");
        return StringToArray(unlockedLevelsString);
    }
    public static void SaveBestTime(int levelIndex, float finishTime)
    {
        float bestTime = GetBestTime(levelIndex);
        if (bestTime == 0f || bestTime > finishTime) 
        {
            PlayerPrefs.SetFloat(levelIndex.ToString(), finishTime);
            PlayerPrefs.Save();
        }
    }
    public static float GetBestTime(int levelIndex)
    {
        float bestTime = PlayerPrefs.GetFloat(levelIndex.ToString(), 0f);
        return bestTime;
    }

    private static string ArrayToString(int[] array)
    {
        return string.Join(",", array);
    }

    private static int[] StringToArray(string str)
    {
        string[] strArray = str.Split(',');
        int[] intArray = new int[strArray.Length];

        for (int i = 0; i < strArray.Length; i++)
        {
            int.TryParse(strArray[i], out intArray[i]);
        }

        return intArray;
    }
}
