using System;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;


public enum RobotnikState
{
    None,
    Fired,
    Working,
    Kaking,
    Coming,
    Leaving
}

public class Robotnik
{
    public int id;
    public RobotnikPropertiesModel robotnikProperties;
    public RobotnikState currentState = RobotnikState.None;
    public event Action robotnikChangedState;

    public Robotnik(int id, RobotnikPropertiesModel robotnikProperties)
    {
        this.id = id;
        this.robotnikProperties = robotnikProperties;
    }

    public void changeState(RobotnikState state)
    {
        currentState = state;
        robotnikChangedState.Invoke();
    }
}
