using System.Collections.Generic;
using UnityEngine;

public class RobotMan : Singleton<RobotMan>
{
    public GameObject robotnikPrefab;
    public Transform spawnPoint;

    [SerializeField] public List<Transform> deskSpawnPoints = new List<Transform>();
    [SerializeField] public List<Transform> toilets = new List<Transform>();

    // Internal Tracking
    private List<bool> deskOccupiedStatus = new List<bool>();
    private Dictionary<GameObject, int> employeeSeatMap = new Dictionary<GameObject, int>();
    private List<GameObject> activeEmployees = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();

        
        for (int i = 0; i < deskSpawnPoints.Count; i++)
        {
            deskOccupiedStatus.Add(false);
        }

        
    }

    public void HireEmployee(Robotnik robotnik)
    {
        List<int> freeIndexes = new List<int>();
        for (int i = 0; i < deskOccupiedStatus.Count; i++)
        {
            if (!deskOccupiedStatus[i]) freeIndexes.Add(i);
        }

        
        if (freeIndexes.Count > 0)
        {
           
            int randomIndex = freeIndexes[Random.Range(0, freeIndexes.Count)];

            
            GameObject newWorker = Instantiate(robotnikPrefab, spawnPoint.position, Quaternion.identity);
            Robotnik newRObotnik = newWorker.AddComponent<Robotnik>();
            newRObotnik.paste(robotnik);
            newRObotnik.setVizual();



            Transform targetDesk = deskSpawnPoints[randomIndex];
            newWorker.GetComponent<ClickToMove>().goal = targetDesk;

            // 6. Track everything
            deskOccupiedStatus[randomIndex] = true;             // Mark seat as taken
            employeeSeatMap.Add(newWorker, randomIndex);       // Map worker to seat index
            activeEmployees.Add(newWorker);                     // Add to master list

            newWorker.GetComponent<ClickToMove>().enabled = true; 
            Debug.Log($"Hired {newWorker.name} at desk #{randomIndex}");
        }
        else
        {
            Debug.LogWarning("Office is full! Build more desks.");
        }
    }

    /// <summary>
    /// Removes the worker and frees up the specific seat they were using.
    /// </summary>
    public void FireEmployee(GameObject employee)
    {
        if (employeeSeatMap.TryGetValue(employee, out int seatIndex))
        {
            // 1. Free the specific seat index
            deskOccupiedStatus[seatIndex] = false;

            // 2. Cleanup tracking
            employeeSeatMap.Remove(employee);
            activeEmployees.Remove(employee);

            // 3. Destroy the object
            Destroy(employee);

            Debug.Log($"Fired employee from desk #{seatIndex}. Seat is now free.");
        }
        else
        {
            Debug.LogError("This worker is not registered in the system!");
        }
    }

    // Optional: Get the count of employees currently working
    public int GetEmployeeCount() => activeEmployees.Count;
}