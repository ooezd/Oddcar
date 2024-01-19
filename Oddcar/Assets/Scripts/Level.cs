using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    public int levelIndex;
    public bool isUnlocked;
    public float bestTime;

    public LevelData(int levelIndex, bool isUnlocked = false, float bestTime = 0f)
    {
        this.levelIndex = levelIndex;
        this.isUnlocked = isUnlocked;
        this.bestTime = bestTime;
    }
}

public class Level : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] Transform[] targets;

    public LevelData data { get; private set; }
    public void SetLevelData(LevelData levelData)
    {
        data = levelData;
    }
    public Transform GetSpawnPoint(int index)
    {
        return spawnPoints[index];
    }

    public Transform GetTarget(int index)
    {
        return targets[index];
    }

    public int GetCarCount()
    {
        return spawnPoints.Length;
    }

    public void SetActiveTarget(int index)
    {
        foreach(Transform t in targets)
        {
            t.gameObject.SetActive(false);
        }

        for(int i = 0; i<=index;i++)
            targets[i].gameObject.SetActive(true);

        foreach (Transform t in spawnPoints)
        {
            t.gameObject.SetActive(false);
        }

        spawnPoints[index].gameObject.SetActive(true);

        if(targets.Length -1 > index+1) 
        {
            spawnPoints[index+1].gameObject.SetActive(true);
        }
    }
}