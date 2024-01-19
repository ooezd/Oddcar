using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    List<Level> levels = new List<Level>();

    private Level loadedLevel;
    public int[] unlockedLevels { get; private set; }
    public event Action<Level> OnLevelCompleted;
    public event Action OnLevelUnloaded;

    public int currentLevelIndex;

    private void Awake()
    {
        LoadData();
    }

    private void LoadData()
    {
        unlockedLevels = PreferencesManager.GetUnlockedLevels();
        for (int i = 0; i < levels.Count; i++)
        {
            bool isLevelUnlocked = unlockedLevels.Contains(i);
            float bestTime = PreferencesManager.GetBestTime(i);
            levels[i].SetLevelData(new LevelData(i, isLevelUnlocked, bestTime));
        }
    }
    private LevelData RefreshData(int levelIndex)
    {
        unlockedLevels = PreferencesManager.GetUnlockedLevels();

        bool isLevelUnlocked = unlockedLevels.Contains(levelIndex);
        float bestTime = PreferencesManager.GetBestTime(levelIndex);
        levels[levelIndex].SetLevelData(new LevelData(levelIndex, isLevelUnlocked, bestTime));

        return levels[levelIndex].data;
    }
    public void OnActiveCarChanged(int carIndex)
    {
        loadedLevel.SetActiveTarget(carIndex);
    }

    public int GetLevelCount()
    {
        return levels.Count;
    }

    public Level GetLoadedLevel()
    {
        return loadedLevel;
    }

    public void LoadLevel(int index)
    {
        if (levels.Count < index)
        {
            Debug.Log($"Level {index} not found!");
            return;
        }

        if (loadedLevel != null)
        {
            UnloadLevel();
        }
        
        var levelToLoad = levels[index];
        loadedLevel = Instantiate(levelToLoad).GetComponent<Level>();
        loadedLevel.SetLevelData(RefreshData(levels[index].data.levelIndex));
        currentLevelIndex = index;
    }
    public bool LoadNextLevelIfExists()
    {
        int nextIndex = currentLevelIndex + 1;

        if (nextIndex >= levels.Count)
            return false;

        LoadLevel(nextIndex);
        return true;
    }

    public void LevelCompleted(float finishTime)
    {
        PreferencesManager.SaveBestTime(loadedLevel.data.levelIndex, finishTime);

        LevelData newData = RefreshData(loadedLevel.data.levelIndex);
        loadedLevel.SetLevelData(newData);

        OnLevelCompleted?.Invoke(loadedLevel);
        
        UnlockLevel();
    }

    public void UnlockLevel()
    {
        var unlockedLevelsList = unlockedLevels.ToList();
        unlockedLevelsList.Add(loadedLevel.data.levelIndex + 1);
        var newUnlockedLevelsArray = unlockedLevelsList.ToArray();
        PreferencesManager.SaveUnlockedLevels(newUnlockedLevelsArray);
    }

    public void UnloadLevel()
    {
        if (loadedLevel != null)
        {
            Destroy(loadedLevel.gameObject);
            loadedLevel = null;
            OnLevelUnloaded?.Invoke();
        }
    }
}
