using System;
using TMPro;
using UnityEngine;

public class TimeDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    void Start()
    {
        GameManager.Instance.OnMinuteChanged += GameManager_OnMinuteChanged;   
    }

    private void GameManager_OnMinuteChanged(int elapsedMinutes)
    {
        timeText.text = TimeSpan.FromMinutes(elapsedMinutes).ToString(@"hh\:mm");
    }
}
