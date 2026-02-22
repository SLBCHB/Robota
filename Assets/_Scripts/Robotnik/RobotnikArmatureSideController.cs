using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Robotnik
{
    class RobotnikArmatureSideController : MonoBehaviour
    {
        [SerializeField] private GameObject Head;
        [SerializeField] private GameObject Body;
        [SerializeField] private GameObject A;
        [SerializeField] private GameObject L;
        [SerializeField] private GameObject Hair;
        [SerializeField] private GameObject Eye;
        public void changeState(int index, ISORobotnikSideArmature armature)
        {
            if (index < 0 || index > 3)
                return;


            transform.localPosition = new Vector3(armature.StateOffset[index].x, armature.StateOffset[index].y, transform.position.z);


            if (armature.Head.Length > index)
                Head.GetComponentInChildren<SpriteRenderer>().sprite = armature.Head[index];
            if (armature.Body.Length > index)
                Body.GetComponentInChildren<SpriteRenderer>().sprite = armature.Body[index];
            if (armature.A.Length > index)
                A.GetComponentInChildren<SpriteRenderer>().sprite = armature.A[index];
            if (armature.L.Length > index)
                L.GetComponentInChildren<SpriteRenderer>().sprite = armature.L[index];
            if (armature.Hair.Length > index)
                Hair.GetComponentInChildren<SpriteRenderer>().sprite = armature.Hair[index];
            if (armature.Eye.Length > index)
                Eye.GetComponentInChildren<SpriteRenderer>().sprite = armature.Eye[index];
        }
    }
}
