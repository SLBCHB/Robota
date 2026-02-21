using Unity.Mathematics.Geometry;
using UnityEngine;

public class RobotnikGenerator : MonoBehaviour
{
    [SerializeField] SORobotnikProbability robotnikProbability;

    private int robotnikCount;

    public void generateRobotnik()
    {
        RobotnikPropertiesModel robotnikProperties = GenerateRobotnikProperties(robotnikProbability);
        Robotnik robotnik = new Robotnik(robotnikCount++, robotnikProperties);   
    }


    private RobotnikPropertiesModel GenerateRobotnikProperties(SORobotnikProbability robotnikProbability)
    {
        RobotnikPropertiesModel robotnikProperties = new RobotnikPropertiesModel();

        float random = Random.Range(0.0f, 1.0f);

        if (random <= robotnikProbability.validCard)
            robotnikProperties.validCard = true;
        else
            robotnikProperties.validCard = false;

        random = Random.Range(0.0f, 1.0f);

        return robotnikProperties;
    }
}
