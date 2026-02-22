using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int gameTimeSpeed;
    [SerializeField] private int fitnessTickSpeedMultiplayer;
    private int minutesElapsed;
    private int earendFitness;

    private float timeAccumulator = 0f;
    private float fitnessTimeAccumulator = 0f;

    public event Action<int> OnMinuteChanged;
    public bool timeStarted = false;
    

    private void Update()
    {
        if (timeStarted)
        {
            timeAccumulator += Time.deltaTime * gameTimeSpeed;
            fitnessTimeAccumulator += Time.deltaTime * gameTimeSpeed * fitnessTickSpeedMultiplayer;
        }

        if (timeAccumulator >= 1f)
        {
            int minutesToAdd = Mathf.FloorToInt(timeAccumulator);
            minutesElapsed += minutesToAdd;

            timeAccumulator -= minutesToAdd;
            OnMinuteChanged?.Invoke(minutesElapsed);
        }

        if(fitnessTimeAccumulator >= 1f)
        {
            int timeToAdd = Mathf.FloorToInt(fitnessTimeAccumulator);
            FitnessTick();
            fitnessTimeAccumulator -= timeToAdd;
        }
    }

    private void FitnessTick()
    {
        foreach(GameObject robotnik in RoomController.Instance.sittingRobotniks)
        {
            if (robotnik == null)
                continue;

            earendFitness += robotnik.GetComponent<Robotnik>().getEarnedProgress();
        }
    }

    public void ResetTimer()
    {
        minutesElapsed = 0;
        timeAccumulator = 0;
    }

    public TimeSpan GetElapsedGameTime()
    {
        return TimeSpan.FromMinutes(minutesElapsed);
    }

    public int getFitness()
    {
        return earendFitness;
    }

    public void addFitness(int amount)
    {
        earendFitness += amount;
    }
}
