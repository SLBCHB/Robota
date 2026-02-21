using System.Collections;
using UnityEngine;

public class NavManMan : MonoBehaviour
{
    [SerializeField] private Transform[] work;
    public GameObject robotak;
    private int count;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        count = RobotnikManager.Instance.robotnici.Count;
        StartCoroutine(SpawnRobotnik());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRobotnik()
    {
        for (int i = 0; i < count; i++)
        {
           RobotnikManager.Instance.robotnici[i].GetComponent<ClickToMove>().goal = work[i];
            RobotnikManager.Instance.robotnici[i].GetComponent<ClickToMove>().enabled = true;
            yield return new WaitForSeconds(1f);
        }
    }
}
