using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum RobotnikState
{
    None,
    Fired,
    Working,
    Kaking,
    Coming,
    Leaving
}

public class Robotnik: MonoBehaviour
{
    public int id;
    public RobotnikPropertiesModel robotnikProperties;
    public RobotnikValidPropertiesModel robotnikValidProperties;
    public RobotnikState currentState = RobotnikState.None;
    public event Action robotnikChangedState;
    public RobotnikType type;
    public RobotnikDirection dir;
    public int State;
    public int HairStyle;


    public void setup(int id, RobotnikPropertiesModel robotnikProperties, RobotnikValidPropertiesModel robotnikValidProperties, RobotnikType type, RobotnikDirection dir, int State, int HairStyle)
    {
        this.id = id;
        this.robotnikProperties = robotnikProperties;
        this.robotnikValidProperties = robotnikValidProperties;
        this.type = type;
        this.dir = dir;
        this.State = State;
        this.HairStyle = HairStyle;
    }

    public void paste(Robotnik robotnik)
    {
        this.id = robotnik.id;
        this.robotnikProperties = robotnik.robotnikProperties;
        this.robotnikValidProperties = robotnik.robotnikValidProperties;
        this.type = robotnik.type;
        this.dir = robotnik.dir;
        this.State = robotnik.State;
        this.HairStyle = robotnik.HairStyle;
    }

    public void setVizual()
    {
        this.GetComponentInChildren<RobotnikSpriteController>().updateCharacter(type, dir, HairStyle, State);
    }

    public void changeState(RobotnikState state)
    {
        currentState = state;
        robotnikChangedState.Invoke();
    }

    public void LogProperties()
    {
        if (robotnikProperties == null)
        {
            Debug.LogWarning($"[Robotnik {id}] Properties jsou NULL!");
            return;
        }

        Debug.Log($"[Robotnik {id}] \n" +
            $"Name: {robotnikProperties.name}\n" +
            $"Card: {robotnikProperties.validCard}\n" +
            $"BirthDate: {robotnikProperties.birthDate}, {robotnikValidProperties.birthDate}\n" +
            $"clockIn: {robotnikProperties.clockInTime}, {robotnikValidProperties.clockInTime}\n" +
            $"clockOut: {robotnikProperties.clockOutTime}, {robotnikValidProperties.clockOutTime}\n");
    }

    public int getEarnedProgress()
    {
        return 1;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Robotnik))]
public class RobotnikEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Robotnik script = (Robotnik)target;
        GUILayout.Space(10);

        if (GUILayout.Button("Log Properties"))
        {
            script.LogProperties();
        }
    }
}
#endif