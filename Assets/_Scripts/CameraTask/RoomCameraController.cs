using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class RoomCameraController : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    [SerializeField] private List<Transform> cameraPos;
    [SerializeField] private List<GameObject> activeUi;

    private int currentCameraPos = 0; 

    void Start()
    {
        
    }


    public void nextCamera()
    {
        switchCamera(currentCameraPos + 1);
    }

    public void previusCamera()
    {
        switchCamera(currentCameraPos - 1);
    }

    public void switchCamera(int cameraPosIndex)
    {
        if (cameraPosIndex < 0)
            currentCameraPos = cameraPos.Count + cameraPosIndex;
        else if (cameraPosIndex >= cameraPos.Count)
            currentCameraPos = cameraPosIndex - cameraPos.Count;
        else
            currentCameraPos = cameraPosIndex;

        camera.transform.position = cameraPos[currentCameraPos].position;

        foreach(GameObject ui in activeUi)
        {
            ui.gameObject.SetActive(false);
        }

        activeUi[currentCameraPos].SetActive(true);
    }
}
