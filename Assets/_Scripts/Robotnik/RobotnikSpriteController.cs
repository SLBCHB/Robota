using Assets._Scripts.Robotnik;
using UnityEngine;

public enum RobotnikDirection
{
    Front,
    Side
}

public enum RobotnikType
{
    Robot,
    Male,
    Gril
}

public class RobotnikSpriteController : MonoBehaviour
{
    public int currentState;
    public int hairStyle;
    public RobotnikDirection currentDir;
    public RobotnikType currentType;

    [SerializeField] RobotnikArmatureFrontController frontController;
    [SerializeField] private ISORobotnikFrontArmature frontArmatureMaleHairStyle1;
    [SerializeField] private ISORobotnikFrontArmature frontArmatureRobotHairStyle1;
    [SerializeField] private ISORobotnikFrontArmature frontArmatureGrilHairStyle1;
    [SerializeField] private ISORobotnikFrontArmature frontArmatureMaleHairStyle2;
    [SerializeField] private ISORobotnikFrontArmature frontArmatureRobotHairStyle2;
    [SerializeField] private ISORobotnikFrontArmature frontArmatureGrilHairStyle2;

    [SerializeField] RobotnikArmatureSideController sideController;
    [SerializeField] private ISORobotnikSideArmature sideArmatureMaleHairStyle1;
    [SerializeField] private ISORobotnikSideArmature sideArmatureRobotHairStyle1;
    [SerializeField] private ISORobotnikSideArmature sideArmatureGrilHairStyle1;
    [SerializeField] private ISORobotnikSideArmature sideArmatureMaleHairStyle2;
    [SerializeField] private ISORobotnikSideArmature sideArmatureRobotHairStyle2;
    [SerializeField] private ISORobotnikSideArmature sideArmatureGrilHairStyle2;

    void Start()
    {
        updateCharacter(currentType, currentDir, currentState);
    }

    public void updateCharacter(RobotnikType type, RobotnikDirection dir, int state)
    {

        currentType = type;
        currentDir = dir;
        currentState = state;
        frontController.gameObject.SetActive(false);
        sideController.gameObject.SetActive(false);

        if (dir == RobotnikDirection.Front)
        {
            frontController.gameObject.SetActive(true);

            if (hairStyle == 0)
            {
                switch (type)
                {
                    case RobotnikType.Male:
                        frontController.changeState(state, frontArmatureMaleHairStyle1);
                        break;
                    case RobotnikType.Robot:
                        frontController.changeState(state, frontArmatureRobotHairStyle1);
                        break;
                    case RobotnikType.Gril:
                        frontController.changeState(state, frontArmatureGrilHairStyle1);
                        break;
                }
            }
            else if(hairStyle == 1)
            {
                switch (type)
                {
                    case RobotnikType.Male:
                        frontController.changeState(state, frontArmatureMaleHairStyle2);
                        break;
                    case RobotnikType.Robot:
                        frontController.changeState(state, frontArmatureRobotHairStyle2);
                        break;
                    case RobotnikType.Gril:
                        frontController.changeState(state, frontArmatureGrilHairStyle2);
                        break;
                }
            }
        }
        else if (dir == RobotnikDirection.Side)
        {
            sideController.gameObject.SetActive(true);

            if (hairStyle == 0)
            {
                switch (type)
                {
                    case RobotnikType.Male:
                        sideController.changeState(state, sideArmatureMaleHairStyle1);
                        break;
                    case RobotnikType.Robot:
                        sideController.changeState(state, sideArmatureRobotHairStyle1);
                        break;
                    case RobotnikType.Gril:
                        sideController.changeState(state, sideArmatureGrilHairStyle1);
                        break;
                }
            }
            else if (hairStyle == 1)
            {
                switch (type)
                {
                    case RobotnikType.Male:
                        sideController.changeState(state, sideArmatureMaleHairStyle2);
                        break;
                    case RobotnikType.Robot:
                        sideController.changeState(state, sideArmatureRobotHairStyle2);
                        break;
                    case RobotnikType.Gril:
                        sideController.changeState(state, sideArmatureGrilHairStyle2);
                        break;
                }
            }
        }
    }
}