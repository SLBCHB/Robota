using System;
using TMPro;
using UnityEngine;

public class RobotnikInfoDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text currentTime;
    [SerializeField] private TMP_Text currentDate;

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text birthDate;
    [SerializeField] private TMP_Text ClockInTime;

    public void displayInfo(Robotnik robotnik)
    {
        nameText.text = robotnik.robotnikProperties.name;

        birthDate.text = robotnik.robotnikProperties.birthDate.ToString("dd. MM. yyyy");
        ClockInTime.text = robotnik.robotnikProperties.clockInTime.ToString(@"hh\:mm");
    }

    public void displayDateTime(DateTime date, TimeSpan time)
    {
        currentDate.text = date.ToString("dd. MM. yyyy");
        currentTime.text = time.ToString(@"hh\:mm");
    }
}
