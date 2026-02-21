using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RobotnikManager : Singleton<RobotnikManager>
{
    public List<GameObject> robotnici;


    public void addRobotnik(GameObject gameObject)
    {
        robotnici.Add(gameObject);
    }


}
