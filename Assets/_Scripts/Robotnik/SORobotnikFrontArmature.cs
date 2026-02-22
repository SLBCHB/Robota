using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using UnityEngine;

[CreateAssetMenu(fileName = "RobotnikFrontArmature", menuName = "ScriptableObjects/RobotnikFrontArmature")]
public class ISORobotnikFrontArmature: ScriptableObject
{
    public Vector2[] StateOffset;
    public Sprite[] Head;
    public Sprite[] Body;
    public Sprite[] RA;
    public Sprite[] LA;
    public Sprite[] RL;
    public Sprite[] LL;
    public Sprite[] Hair;
    public Sprite[] Eyes;
}

