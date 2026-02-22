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

    private GameObject _activeCard;

    public void HandleSubjectProcessed(SubjectEntity processedSubject)
    {
        if (processedSubject != null)
        {
            Destroy(processedSubject.gameObject, destroyDelay);
        }
        
        if (_activeCard != null)
        {
            Destroy(_activeCard);
        }

        StartCoroutine(AdvanceQueueRoutine());
    }

    private IEnumerator AdvanceQueueRoutine()
    {
        yield return new WaitForSeconds(delayBeforeMove);

        if (_queueList.Count > 0)
        {
            _activeSubject = _queueList[0];
            _queueList.RemoveAt(0); 
            
            if (_activeSubject != null && activeSpot != null)
            {
                _activeSubject.MoveToNewSpot(activeSpot); 
                _activeSubject.SetInteractable(true); 
                
                StartCoroutine(WaitAndTossCard(_activeSubject));
            }

            for (int i = 0; i < _queueList.Count; i++)
            {
                if (_queueList[i] != null && queueSpots[i] != null)
                {
                    _queueList[i].MoveToNewSpot(queueSpots[i]);
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
                }
            }
        }
    }

    private IEnumerator WaitAndTossCard(SubjectEntity subject)
    {
        yield return new WaitForSeconds(0.5f);
        if (subject != null)
        {
            _activeCard = subject.TossIDCard();
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