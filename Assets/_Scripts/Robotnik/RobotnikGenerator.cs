using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Audio.GeneratorInstance;

public class RobotnikGenerator : MonoBehaviour
{
    [SerializeField] private SubjectManager inLineController;
    [SerializeField] private Transform inLinePreSpawn;
    [SerializeField] private GameObject InLinePrefab;
    private SORobotnikProbability currentRobotnikProbability;
    private SODifficulty currentDifficulty;
    private SORobotnikProperitesCap currentRobotnikProperitesCap;

    private readonly string[] firstNames = { "Pepa", "Franta", "Karel", "Lojza", "Jarda", "Milan", "Zdeněk", "Petr", "Honza" };
    private readonly string[] lastNames = { "Novák", "Svoboda", "Novotný", "Dvořák", "Černý", "Procházka", "Kučera", "Veselý" };

    private void Start()
    {
        generateRobotniksForLine();
        inLineController.Init();

    }


 

    public void generateRobotniksForLine()
    {
        currentDifficulty = DifficultyManager.Instance.getCurrentDifficulty();
        currentRobotnikProbability = currentDifficulty.robotnikProbability;
        currentRobotnikProperitesCap = currentDifficulty.robotnikProperitesCap;

        for(int i = 0; i < currentDifficulty.RobotnikCount; i++)
        {
            var (props, validProps) = GenerateRobotnikProperties(currentRobotnikProbability, currentRobotnikProperitesCap);

            GameObject gameObject = Instantiate(InLinePrefab, inLinePreSpawn);
            Robotnik robotnik = gameObject.GetComponent<Robotnik>();
            if(robotnik == null)
            {
                robotnik = gameObject.AddComponent<Robotnik>();
            }
            int type = Random.Range(0, 3);
            int hairStyle = Random.Range(0, 2);
            robotnik.setup(i, props, validProps, (RobotnikType)type, RobotnikDirection.Side, 0, hairStyle);
            robotnik.setVizual();
            inLineController._prequeList.Add(gameObject);
        }
    }


    public void RegenRobotnikWorRoom(Robotnik robotnik)
    {
        RobotMan.Instance.HireEmployee(robotnik);
    }

    private (RobotnikPropertiesModel properties, RobotnikValidPropertiesModel validProperties) GenerateRobotnikProperties(SORobotnikProbability robotnikProbability, SORobotnikProperitesCap robotnikProperitesCap)
    {
        RobotnikPropertiesModel robotnikProperties = new RobotnikPropertiesModel();
        RobotnikValidPropertiesModel robotnikValidProperties = new RobotnikValidPropertiesModel();

        string randomFirst = firstNames[Random.Range(0, firstNames.Length)];
        string randomLast = lastNames[Random.Range(0, lastNames.Length)];

        robotnikProperties.name = $"{randomFirst} {randomLast}";

        float randomCardValidity = Random.Range(0.0f, 1.0f);

        //CARD
        if (randomCardValidity <= robotnikProbability.validCard)
        {
            robotnikProperties.validCard = true;
            robotnikValidProperties.validCard = true;
        }
        else
        {
            robotnikProperties.validCard = false;
            robotnikValidProperties.validCard = false;
        }

        //AGE
        float randomAge = Random.Range(0.0f, 1.0f);

        if (randomAge <= robotnikProbability.rightAge)
        {
            int validDaysRange = (robotnikProperitesCap.maxBirthDate - robotnikProperitesCap.minBirthDate).Days;

            int randomDays = Random.Range(0, validDaysRange + 1);
            robotnikProperties.birthDate = robotnikProperitesCap.minBirthDate.AddDays(randomDays);
            robotnikValidProperties.birthDate = true;
        }
        else
        {
            robotnikValidProperties.birthDate = false;
            bool isTooYoung = Random.value > 0.5f;

            if (isTooYoung)
            {
                int invalidYoungRange = (currentDifficulty.currentDate - robotnikProperitesCap.maxBirthDate).Days;

                if (invalidYoungRange > 0)
                {
                    int randomDays = Random.Range(1, invalidYoungRange);
                    robotnikProperties.birthDate = robotnikProperitesCap.maxBirthDate.AddDays(randomDays);
                }
                else
                {
                    robotnikProperties.birthDate = robotnikProperitesCap.maxBirthDate.AddDays(365 * 2);
                }
            }
            else
            {
                int randomDaysBefore = Random.Range(1, 365 * 20);
                robotnikProperties.birthDate = robotnikProperitesCap.minBirthDate.AddDays(-randomDaysBefore);
            }
        }

        //ClockIN
        float randomClockIn = Random.value;
        int minInMinutes = (int)robotnikProperitesCap.minClockIn.TotalMinutes;
        int maxInMinutes = (int)robotnikProperitesCap.maxClockIn.TotalMinutes;

        if (randomClockIn <= robotnikProbability.clockInInTime)
        {
            int randomMinutes = Random.Range(minInMinutes, maxInMinutes + 1);
            robotnikProperties.clockInTime = System.TimeSpan.FromMinutes(randomMinutes);
            robotnikValidProperties.clockInTime = true;
        }
        else
        {
            robotnikValidProperties.clockInTime = false;
            bool isTooEarly = Random.value > 0.5f;

            if (isTooEarly)
            {
                int earlyMinutes = Random.Range(Mathf.Max(0, minInMinutes - 120), minInMinutes);
                robotnikProperties.clockInTime = System.TimeSpan.FromMinutes(earlyMinutes);
            }
            else
            {
   
                int lateMinutes = Random.Range(maxInMinutes + 1, maxInMinutes + 121);
                robotnikProperties.clockInTime = System.TimeSpan.FromMinutes(lateMinutes);
            }
        }

        //ClockOut
        float randomClockOut = UnityEngine.Random.value;

        int minOutMinutes = (int)robotnikProperitesCap.minClockOut.TotalMinutes;
        int maxOutMinutes = (int)robotnikProperitesCap.maxClockOut.TotalMinutes;

        if (randomClockOut <= robotnikProbability.clockOutInTime)
        {
            int randomMinutes = UnityEngine.Random.Range(minOutMinutes, maxOutMinutes + 1);
            robotnikProperties.clockOutTime = System.TimeSpan.FromMinutes(randomMinutes);
            robotnikValidProperties.clockOutTime = true;
        }
        else
        {
            robotnikValidProperties.clockOutTime = false;
            bool leftTooEarly = UnityEngine.Random.value > 0.5f;

            if (leftTooEarly)
            {
                int earlyOutMinutes = Random.Range(minOutMinutes - 120, minOutMinutes);
                robotnikProperties.clockOutTime = System.TimeSpan.FromMinutes(earlyOutMinutes);
            }
            else
            {
                int lateOutMinutes = Random.Range(maxOutMinutes + 1, Mathf.Min(1440, maxOutMinutes + 121));
                robotnikProperties.clockOutTime = System.TimeSpan.FromMinutes(lateOutMinutes);
            }
        }


        return (robotnikProperties, robotnikValidProperties);
    }
}
