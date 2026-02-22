using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Robotnik
{
    class RobotnikArmatureFrontController : MonoBehaviour
    {
        [SerializeField] private GameObject Head;
        [SerializeField] private GameObject Body;
        [SerializeField] private GameObject RA;
        [SerializeField] private GameObject LA;
        [SerializeField] private GameObject RL;
        [SerializeField] private GameObject LL;
        [SerializeField] private GameObject Hair;
        [SerializeField] private GameObject Eyes;
        public void changeState(int index, ISORobotnikFrontArmature armature)
        {
            if (index < 0 || index > 3)
                return;


            transform.localPosition = new Vector3(armature.StateOffset[index].x, armature.StateOffset[index].y, transform.position.z);


            if (armature.Head.Length > index)
                Head.GetComponentInChildren<SpriteRenderer>().sprite = armature.Head[index];
            if (armature.Body.Length > index)
                Body.GetComponentInChildren<SpriteRenderer>().sprite = armature.Body[index];
            if (armature.RA.Length > index)
                RA.GetComponentInChildren<SpriteRenderer>().sprite = armature.RA[index];
            if (armature.LA.Length > index)
                LA.GetComponentInChildren<SpriteRenderer>().sprite = armature.LA[index];
            if (armature.RL.Length > index)
                RL.GetComponentInChildren<SpriteRenderer>().sprite = armature.RL[index];
            if (armature.LL.Length > index)
                LL.GetComponentInChildren<SpriteRenderer>().sprite = armature.LL[index];
            if (armature.Hair.Length > index)
                Hair.GetComponentInChildren<SpriteRenderer>().sprite = armature.Hair[index];
            if (armature.Eyes.Length > index)
                Eyes.GetComponentInChildren<SpriteRenderer>().sprite = armature.Eyes[index];
        }
    }
}
