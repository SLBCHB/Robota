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

    public void setup(int id, RobotnikPropertiesModel robotnikProperties, RobotnikValidPropertiesModel robotnikValidProperties)
    {
        this.id = id;
        this.robotnikProperties = robotnikProperties;
        this.robotnikValidProperties = robotnikValidProperties;
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