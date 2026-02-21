using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Difficulty", menuName = "ScriptableObjects/Difficulty")]
public class SODifficulty : ScriptableObject
{
    public int day;
    public int RobotnikCount;
    [Header("Current Date (Year, Month, Day)")]
    [SerializeField] private Vector3Int currentDateEditor = new Vector3Int(1982, 10, 24);
    public DateTime currentDate => new DateTime(currentDateEditor.x, currentDateEditor.y, currentDateEditor.z);

    [Header("Current Time (Hours, Minutes)")]
    [SerializeField] private Vector2Int currentTimeEditor = new Vector2Int(6, 0);

    public TimeSpan currentTime => new TimeSpan(currentTimeEditor.x, currentTimeEditor.y, 0);


    public SORobotnikProbability robotnikProbability;
    public SORobotnikProperitesCap robotnikProperitesCap;
}
