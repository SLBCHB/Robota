using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubjectManager : MonoBehaviour
{
    [Header("Positions")]
    public Transform activeSpot;
    public Transform[] queueSpots; 

    [Header("Spawning Setup")]
    public GameObject[] subjectPrefabs; 

    [Header("Timing")]
    public float delayBeforeMove = 1.0f;
    public float destroyDelay = 3.0f; 

    private SubjectEntity _activeSubject;
    private List<SubjectEntity> _queueList = new List<SubjectEntity>();

    private void Start()
    {
        _activeSubject = SpawnSubjectAt(activeSpot);
        if (_activeSubject != null) _activeSubject.SetInteractable(true);

        for (int i = 0; i < queueSpots.Length; i++)
        {
            SubjectEntity qPerson = SpawnSubjectAt(queueSpots[i]);
            if (qPerson != null) qPerson.SetInteractable(false); 
            _queueList.Add(qPerson);
        }
    }

    public void HandleSubjectProcessed(SubjectEntity processedSubject)
    {
        if (processedSubject != null)
        {
            Destroy(processedSubject.gameObject, destroyDelay);
        }
        StartCoroutine(AdvanceQueueRoutine());
    }

    private IEnumerator AdvanceQueueRoutine()
    {
        yield return new WaitForSeconds(delayBeforeMove);

        Debug.Log($"<color=cyan>--- ADVANCING QUEUE ---</color>");

        if (_queueList.Count > 0)
        {
            _activeSubject = _queueList[0];
            _queueList.RemoveAt(0); 
            
            if (_activeSubject != null && activeSpot != null)
            {
                Debug.Log($"<color=green>Moving {_activeSubject.gameObject.name} to Active Spot</color>");
                _activeSubject.MoveToNewSpot(activeSpot); 
                _activeSubject.SetInteractable(true); 
            }

            for (int i = 0; i < _queueList.Count; i++)
            {
                if (_queueList[i] != null && queueSpots[i] != null)
                {
                    Debug.Log($"<color=orange>Moving {_queueList[i].gameObject.name} forward to {queueSpots[i].gameObject.name}</color>");
                    _queueList[i].MoveToNewSpot(queueSpots[i]);
                }
                else
                {
                    Debug.LogError($"<color=red>ERROR: Queue List or Queue Spot at index {i} is missing!</color>");
                }
            }

            if (queueSpots.Length > 0)
            {
                Transform lastSpot = queueSpots[queueSpots.Length - 1];
                SubjectEntity newPerson = SpawnSubjectAt(lastSpot);
                if (newPerson != null) 
                {
                    newPerson.SetInteractable(false);
                    _queueList.Add(newPerson);
                    Debug.Log($"<color=yellow>Spawned new person at the back of the line.</color>");
                }
            }
        }
        else
        {
            Debug.LogWarning("Queue list was empty, couldn't advance!");
        }
    }

    private SubjectEntity SpawnSubjectAt(Transform spot)
    {
        if (subjectPrefabs == null || subjectPrefabs.Length == 0 || spot == null) return null;

        int randomIndex = Random.Range(0, subjectPrefabs.Length);
        GameObject newObj = Instantiate(subjectPrefabs[randomIndex], spot.position, Quaternion.identity);
        
        SubjectEntity entity = newObj.GetComponent<SubjectEntity>();
        if (entity != null)
        {
            entity.basePosition = spot;
        }
        
        return entity;
    }
}