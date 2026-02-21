using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : Singleton<DifficultyManager>
{
    [SerializeField] private List<SODifficulty> days;
    public int dayCounter;


    public SODifficulty getCurrentDifficulty()
    {
        return days[dayCounter];
    }

    public void nextDay()
    {
        dayCounter++;
    }
}
