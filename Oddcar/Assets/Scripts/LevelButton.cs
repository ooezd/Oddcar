using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : Button
{
    TextMeshProUGUI buttonText;

    int levelIndex;
    Action<int> onSelected;

    protected override void Awake()
    {
        onClick.AddListener(SelectLevel);
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Init(int index, bool isUnlocked, Action<int> onSelected)
    {
        this.levelIndex = index;
        this.onSelected = onSelected;

        buttonText.text = $"LEVEL {levelIndex + 1}";
        interactable = isUnlocked;
    }

    private void SelectLevel()
    {
        onSelected?.Invoke(levelIndex);
    }

    protected override void OnDestroy()
    {
        onClick.RemoveListener(SelectLevel);
    }
}