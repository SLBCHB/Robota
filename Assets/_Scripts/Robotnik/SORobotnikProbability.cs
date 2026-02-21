using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using UnityEngine;

[CreateAssetMenu(fileName = "RobotnikProbability", menuName = "ScriptableObjects/RobotnikProbability")]
public class SORobotnikProbability: ScriptableObject
{
    public float validCard;
    public float rightAge;
    public float clockInInTime;
    public float clockOutInTime;
}

