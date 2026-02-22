using System.Collections.Generic;
using UnityEngine;

public class RoomController : Singleton<RoomController>
{
    [SerializeField] private List<Transform> deskSpawnPoints = new List<Transform>();

    public GameObject[] sittingRobotniks;

    protected override void Awake()
    {
        base.Awake();
        sittingRobotniks = new GameObject[deskSpawnPoints.Count];
    }

    public int getDeskCount()
    {
        return deskSpawnPoints.Count;
    }

    public int getSittingCount()
    {
        int count = 0;
        for (int i = 0; i < sittingRobotniks.Length; i++)
        {
            if (sittingRobotniks[i] != null)
            {
                count++;
            }
        }

        return count;
    }

    public void sitNextRobotnik(GameObject robotnik)
    {
        for (int i = 0; i < sittingRobotniks.Length; i++)
        {
            if (sittingRobotniks[i] == null)
            {
                sittingRobotniks[i] = robotnik;
                robotnik.transform.position = deskSpawnPoints[i].position;

                return;
            }
        }
    }

    public void removeRobotnik(int deskIndex)
    {
        if (deskIndex < 0 || deskIndex >= sittingRobotniks.Length)
        {
            Debug.LogWarning($"[RoomController] Stůl s indexem {deskIndex} neexistuje!");
            return;
        }

        if (sittingRobotniks[deskIndex] != null)
        {
            GameObject robotnikToLeave = sittingRobotniks[deskIndex];
            sittingRobotniks[deskIndex] = null;
            robotnikToLeave.SetActive(false);
        }
    }
}