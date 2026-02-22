using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using UnityEngine;

[CreateAssetMenu(fileName = "RobotnikSideArmature", menuName = "ScriptableObjects/RobotnikSideArmature")]
public class ISORobotnikSideArmature: ScriptableObject
{
    public Vector2[] StateOffset;
    public Sprite[] Head;
    public Sprite[] Body;
    public Sprite[] A;
    public Sprite[] L;
    public Sprite[] Hair;
    public Sprite[] Eye;
}

