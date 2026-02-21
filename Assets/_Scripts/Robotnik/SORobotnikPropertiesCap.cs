using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Difficulty", menuName = "ScriptableObjects/RobotnikPropertiesCap")]
public class SORobotnikProperitesCap : ScriptableObject
{
    [Header("Age Constraints")]
    public int minAge;
    public int maxAge;

    // --- DATUM NAROZENÍ ---
    // Pro Inspector použijeme Vector3Int (X = Rok, Y = Měsíc, Z = Den)
    [Header("Birth Date (Year, Month, Day)")]
    [SerializeField] private Vector3Int minBirthDateEditor = new Vector3Int(1950, 1, 1);
    [SerializeField] private Vector3Int maxBirthDateEditor = new Vector3Int(2005, 12, 31);

    // Tyhle Properties si tvůj generátor automaticky převede na DateTime
    public DateTime minBirthDate => new DateTime(minBirthDateEditor.x, minBirthDateEditor.y, minBirthDateEditor.z);
    public DateTime maxBirthDate => new DateTime(maxBirthDateEditor.x, maxBirthDateEditor.y, maxBirthDateEditor.z);


    // --- ČAS PŘÍCHODU (Clock In) ---
    // Pro Inspector použijeme Vector2Int (X = Hodiny, Y = Minuty)
    [Header("Clock In Time (Hours, Minutes)")]
    [SerializeField] private Vector2Int minClockInEditor = new Vector2Int(6, 0);
    [SerializeField] private Vector2Int maxClockInEditor = new Vector2Int(8, 0);

    public TimeSpan minClockIn => new TimeSpan(minClockInEditor.x, minClockInEditor.y, 0);
    public TimeSpan maxClockIn => new TimeSpan(maxClockInEditor.x, maxClockInEditor.y, 0);


    // --- ČAS ODCHODU (Clock Out) ---
    [Header("Clock Out Time (Hours, Minutes)")]
    [SerializeField] private Vector2Int minClockOutEditor = new Vector2Int(14, 0);
    [SerializeField] private Vector2Int maxClockOutEditor = new Vector2Int(16, 0);

    public TimeSpan minClockOut => new TimeSpan(minClockOutEditor.x, minClockOutEditor.y, 0);
    public TimeSpan maxClockOut => new TimeSpan(maxClockOutEditor.x, maxClockOutEditor.y, 0);
}